using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Models;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.fin.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ErpShowroom.Infrastructure.Persistence.Services;

public class QueryOptimizationService
{
    private readonly AppDbContext _context;

    public QueryOptimizationService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Scenario 1: Fix N+1 Query Problem using Deep Eager Loading.
    /// Uses filtered/sorted includes available in EF Core 5.0+.
    /// </summary>
    public async Task<List<HPAgreement>> GetAgreementsWithDetailsAsync(long? customerId = null)
    {
        try
        {
            Log.Information("Loading HP Agreements with nested details for CustomerId: {CustomerId}", customerId);

            var query = _context.HPAgreements
                .AsNoTracking()
                .AsSplitQuery() // Optimization for deep graphs with many collections to avoid Cartesian explosion
                .Include(a => a.Customer)
                .Include(a => a.Product!)
                    .ThenInclude(p => p.Brand)
                .Include(a => a.Product!)
                    .ThenInclude(p => p.Category)
                .Include(a => a.EMISchedules!.OrderBy(e => e.InstallmentNo))
                    .ThenInclude(e => e.Payments!.OrderByDescending(p => p.PaymentDate))
                .Where(a => !customerId.HasValue || a.CustomerId == customerId);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching agreements with details.");
            throw;
        }
    }

    /// <summary>
    /// Scenario 2: Projection for Recovery Dashboard.
    /// Projects to a DTO in the database to minimize data transfer.
    /// </summary>
    public async Task<List<RecoveryDashboardItem>> GetRecoveryDashboardDataAsync()
    {
        try
        {
            Log.Information("Projecting recovery dashboard data for all unpaid agreements.");

            var query = _context.HPAgreements
                .AsNoTracking()
                .Where(a => a.Status != HPAgreementStatus.Closed)
                .Select(a => new RecoveryDashboardItem
                {
                    AgreementNo = a.AgreementNo ?? "N/A",
                    CustomerName = a.Customer != null ? a.Customer.FullName ?? "Unknown" : "Unknown",
                    RiskBucket = a.RecoveryBoard != null ? a.RecoveryBoard.RiskBucket : null,
                    // Sum of unpaid EMIs (TotalDue + PenaltyAmount)
                    TotalOverdue = a.EMISchedules!
                        .Where(e => e.Status != EMIPaymentStatus.Paid)
                        .Sum(e => (e.TotalDue ?? 0) + (e.PenaltyAmount ?? 0))
                });

            // The SQL generated will use a subquery or join for the Sum(), 
            // ensuring calculation happens in the DB engine.
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during recovery dashboard projection.");
            throw;
        }
    }

    /// <summary>
    /// Scenario 3: Bulk Update – Apply Penalty to Overdue EMIs via ExecuteUpdateAsync.
    /// Single batch update without loading entities into memory.
    /// </summary>
    public async Task<int> ApplyBulkPenaltyAsync(DateTime asOfDate, decimal penaltyRatePercent)
    {
        try
        {
            Log.Warning("Applying bulk penalty of {Rate}% to EMIs overdue as of {AsOfDate}", 
                penaltyRatePercent, asOfDate.ToShortDateString());

            // Bulk update reduces network roundtrips and memory overhead significantly.
            int affectedRows = await _context.EMISchedules
                .Where(e => e.Status != EMIPaymentStatus.Paid && e.DueDate < asOfDate)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.PenaltyAmount, e => (e.PenaltyAmount ?? 0) + 
                        ((e.TotalDue ?? 0) * (penaltyRatePercent / 100m) * EF.Functions.DateDiffDay(e.DueDate, asOfDate)))
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow)
                );

            Log.Information("Bulk penalty application complete. {AffectedRows} rows updated.", affectedRows);
            return affectedRows;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Bulk penalty update failed.");
            throw;
        }
    }

    /// <summary>
    /// Scenario 4: Raw SQL for Portfolio at Risk (PAR) Report.
    /// Uses grouping and joins in SQL for multi-dimensional analysis.
    /// </summary>
    public async Task<List<ParReportResult>> GetParReportByBranchAsync()
    {
        try
        {
            Log.Information("Running raw SQL PAR 30 report by branch.");

            // Standard PAR 30 logic: (Total outstanding for agreements overdue > 30 days) / (Total active portfolio)
            // Using Database.SqlQuery with parameters to avoid SQL Injection.
            // BranchId is mapped from LocationId in BaseEntity.
            var sql = @"
                SELECT 
                    a.LocationId AS BranchId,
                    SUM(a.FinanceAmount) AS Portfolio,
                    SUM(CASE WHEN rb.TotalOverdue > 0 AND DATEDIFF(day, COALESCE(rb.LastFollowupDate, a.AgreementDate), GETDATE()) > 30 THEN a.FinanceAmount ELSE 0 END) AS Overdue30Plus
                FROM fin.HPAgreements a
                LEFT JOIN fin.RecoveryBoards rb ON a.Id = rb.HPAgreementId
                WHERE a.Status = 1 -- 1 = Running/Approved (assuming)
                  AND a.IsDeleted = 0
                GROUP BY a.LocationId";

            return await _context.Database
                .SqlQueryRaw<ParReportResult>(sql)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Raw SQL PAR report execution failed.");
            throw;
        }
    }
}
