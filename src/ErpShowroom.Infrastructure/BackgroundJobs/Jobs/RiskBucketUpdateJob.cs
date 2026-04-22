using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Infrastructure.BackgroundJobs.Jobs;

public class RiskBucketUpdateJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RiskBucketUpdateJob> _logger;

    public RiskBucketUpdateJob(IApplicationDbContext context, ILogger<RiskBucketUpdateJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ExecuteAsync()
    {
        try
        {
            var today = DateTime.UtcNow.Date;

            // Using pure EF Core logic but structured to allow query-time calculation where possible.
            // We fetch active agreements.
            var activeAgreementsQuery = _context.HPAgreements
                .Include(a => a.RecoveryBoard)
                .Where(a => a.Status == HPAgreementStatus.Active)
                .AsTracking();

            var agreements = await activeAgreementsQuery.ToListAsync();

            int count = 0;

            foreach (var agreement in agreements)
            {
                var unpaidEmis = await _context.EMISchedules
                    .Where(e => e.HPAgreementId == agreement.Id && e.Status != EMIPaymentStatus.Paid && e.DueDate < today)
                    .ToListAsync();

                decimal totalOverdue = unpaidEmis.Sum(e => e.TotalDue ?? 0);
                int maxDaysOverdue = unpaidEmis.Any() 
                    ? unpaidEmis.Max(e => (today - e.DueDate.GetValueOrDefault(today)).Days) 
                    : 0;

                RiskBucketEnum riskBucket = DetermineRiskBucket(maxDaysOverdue);

                if (agreement.RecoveryBoard == null)
                {
                    agreement.RecoveryBoard = new Domain.fin.Entities.RecoveryBoard
                    {
                        HPAgreementId = agreement.Id,
                        IsActive = true
                    };
                    _context.RecoveryBoards.Add(agreement.RecoveryBoard);
                }

                agreement.RecoveryBoard.RiskBucket = riskBucket;
                agreement.RecoveryBoard.TotalOverdue = totalOverdue;
                agreement.RecoveryBoard.UpdatedAt = DateTime.UtcNow;

                count++;
                if (count % 100 == 0)
                {
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("RiskBucketUpdateJob completed. Updated {Count} agreements.", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RiskBucketUpdateJob failed.");
            throw;
        }
    }

    private static RiskBucketEnum DetermineRiskBucket(int maxDaysOverdue)
    {
        if (maxDaysOverdue == 0) return RiskBucketEnum.Current;
        if (maxDaysOverdue <= 30) return RiskBucketEnum.Days1_30;
        if (maxDaysOverdue <= 60) return RiskBucketEnum.Days31_60;
        if (maxDaysOverdue <= 90) return RiskBucketEnum.Days61_90;
        return RiskBucketEnum.Days90Plus;
    }
}
