using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.BackgroundJobs.Jobs;

public class SalaryProcessJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SalaryProcessJob> _logger;

    public SalaryProcessJob(IApplicationDbContext context, ILogger<SalaryProcessJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ExecuteAsync()
    {
        try
        {
            // Determine the target processing month automatically. 
            // Running on 25th, calculate for previous month
            var now = DateTime.UtcNow;
            var targetMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1);

            int workingDaysInMonth = 26; // Simplified: 26 working days fixed.

            var activeEmployees = await _context.Employees
                .Where(e => e.IsActive == true && e.TerminationDate == null)
                .ToListAsync();

            int count = 0;

            foreach (var emp in activeEmployees)
            {
                // Verify if SalarySlip already exists for target month (Idempotency check)
                bool slipExists = await _context.SalarySlips
                    .AnyAsync(s => s.EmployeeId == emp.Id && s.MonthYear == targetMonth);

                if (slipExists) continue;

                // Grab latest salary structure active before/on this target month
                var structure = await _context.Set<ErpShowroom.Domain.prl.Entities.SalaryStructure>()
                    .Where(s => s.EmployeeId == emp.Id && s.EffectiveFrom <= targetMonth)
                    .OrderByDescending(s => s.EffectiveFrom)
                    .FirstOrDefaultAsync();

                if (structure == null) continue;

                // Calculate attendances for the target month
                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == emp.Id && a.Date >= targetMonth && a.Date < targetMonth.AddMonths(1))
                    .ToListAsync();

                decimal presentDays = attendances.Count(a => a.AttendanceStatus == "Present");
                decimal halfDays = attendances.Count(a => a.AttendanceStatus == "HalfDay");

                decimal effectiveDays = presentDays + (halfDays * 0.5m);
                // Optionally adjust gross based on effectiveDays/workingDaysInMonth, but we simplify to absolute base metrics
                // if prompt doesn't specify pro-ration precisely, we'll calculate literal gross.
                // Assuming typical proration:
                decimal proRationFactor = workingDaysInMonth > 0 ? (Math.Min(effectiveDays, workingDaysInMonth) / workingDaysInMonth) : 1;

                decimal basic = (structure.Basic ?? 0) * proRationFactor;
                decimal grossSalary = basic 
                                    + (structure.HouseRent ?? 0) 
                                    + (structure.Medical ?? 0) 
                                    + (structure.Transport ?? 0) 
                                    + (structure.OtherAllowance ?? 0);

                // Deductions logic
                decimal pf = basic * ((structure.ProvidentFundPct ?? 0) / 100m);
                decimal tax = grossSalary * ((structure.TaxDeductionPct ?? 0) / 100m);
                decimal totalDeductions = pf + tax;
                decimal netPayable = grossSalary - totalDeductions;

                var slip = new ErpShowroom.Domain.prl.Entities.SalarySlip
                {
                    EmployeeId = emp.Id,
                    MonthYear = targetMonth,
                    Basic = basic,
                    GrossSalary = grossSalary,
                    TotalDeductions = totalDeductions,
                    NetPayable = netPayable,
                    Status = "Processed", // SalaryStatus.Processed
                    PaymentDate = null,
                    IsActive = true
                };

                _context.SalarySlips.Add(slip);

                count++;
                if (count % 100 == 0)
                {
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("SalaryProcessJob executed. Processed {Count} slips.", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SalaryProcessJob failed.");
            throw;
        }
    }
}

namespace ErpShowroom.Domain.prl.Entities
{
    // Provide explicit fallback properties just in case
    public partial class SalaryStructure : ErpShowroom.Domain.Common.BaseEntity
    {
        public long? EmployeeId { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public decimal? Basic { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Transport { get; set; }
        public decimal? OtherAllowance { get; set; }
        public decimal? ProvidentFundPct { get; set; }
        public decimal? TaxDeductionPct { get; set; }
    }
    
    public partial class SalarySlip : ErpShowroom.Domain.Common.BaseEntity
    {
        public long? EmployeeId { get; set; }
        public DateTime? MonthYear { get; set; }
        public decimal? Basic { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? NetPayable { get; set; }
        public string? Status { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}

namespace ErpShowroom.Domain.hr.Entities
{
    public partial class Employee : ErpShowroom.Domain.Common.BaseEntity
    {
        public DateTime? TerminationDate { get; set; }
    }
    
    public partial class Attendance : ErpShowroom.Domain.Common.BaseEntity
    {
        public long? EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public string? AttendanceStatus { get; set; }
    }
}
