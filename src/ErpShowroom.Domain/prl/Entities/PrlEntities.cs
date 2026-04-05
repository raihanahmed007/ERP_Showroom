using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.hr.Entities;

namespace ErpShowroom.Domain.prl.Entities;

public class SalaryStructure : BaseEntity
{
    public long? EmployeeId { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public decimal? Basic { get; set; }
    public decimal? HouseRent { get; set; }
    public decimal? Medical { get; set; }
    public decimal? Transport { get; set; }
    public decimal? OtherAllowance { get; set; }
    public decimal? ProvidentFundPct { get; set; } = 10;
    public decimal? TaxDeductionPct { get; set; } = 0;

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}

public class SalarySlip : BaseEntity
{
    public long? EmployeeId { get; set; }
    public DateTime? MonthYear { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? TotalDeductions { get; set; }
    public decimal? NetPayable { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMode { get; set; }
    public string? BankTransactionNo { get; set; }
    public SalaryStatus? Status { get; set; } = SalaryStatus.Draft;
    public string? Remarks { get; set; }
    public long? ProcessedByUserId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}

public class Incentive : BaseEntity
{
    public long? EmployeeId { get; set; }
    public DateTime? IncentiveMonth { get; set; }
    public IncentiveTypeEnum? IncentiveType { get; set; }
    public decimal? TargetAmount { get; set; }
    public decimal? AchievedAmount { get; set; }
    public decimal? IncentiveEarned { get; set; }
    public long? PaidInSalarySlipId { get; set; }
    public string? CalculationFormula { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}
