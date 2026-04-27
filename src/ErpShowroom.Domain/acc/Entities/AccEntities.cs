using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.acc.Entities;

public class ChartOfAccount : BaseEntity
{
    [Required, MaxLength(50)] public string? AccountCode { get; set; }
    [Required, MaxLength(200)] public string? AccountName { get; set; }
    public AccountTypeEnum? AccountType { get; set; }
    public long? ParentAccountId { get; set; }
    public bool? IsHead { get; set; } = false;
    public string? CurrencyCode { get; set; } = "BDT";
    public decimal? OpeningBalance { get; set; }
    public string? Notes { get; set; }

    [ForeignKey(nameof(ParentAccountId))]
    public virtual ChartOfAccount? ParentAccount { get; set; }

    [InverseProperty(nameof(ParentAccount))]
    public virtual ICollection<ChartOfAccount>? ChildAccounts { get; set; }

    public virtual ICollection<JournalLine>? JournalLines { get; set; }
}

public class FiscalPeriod : BaseEntity
{
    [Required, MaxLength(50)] public string? PeriodName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsClosed { get; set; } = false;
    public long? ClosedByUserId { get; set; }
    public DateTime? ClosedAt { get; set; }
    public long? NextPeriodId { get; set; }
}

public class JournalEntry : BaseEntity
{
    [Required, MaxLength(50)] public string? VoucherNo { get; set; }
    public VoucherTypeEnum? VoucherType { get; set; }
    public DateTime? JournalDate { get; set; }
    public long? PeriodId { get; set; }
    public string? Narration { get; set; }
    public VoucherStatus Status { get; set; } = VoucherStatus.Draft;
    
    public bool? IsReversed { get; set; } = false;
    public long? ReversedFromJournalId { get; set; }
    
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    public long? PostedByUserId { get; set; }
    public DateTime? PostedAt { get; set; }

    public string? ReferenceNo { get; set; }
    public decimal? TotalDebit { get; set; }
    public decimal? TotalCredit { get; set; }

    [ForeignKey(nameof(PeriodId))]
    public virtual FiscalPeriod? Period { get; set; }

    public virtual ICollection<JournalLine>? JournalLines { get; set; }

    public void Post(long userId)
    {
        if (Status != VoucherStatus.Draft)
            throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Only draft vouchers can be posted.");
        
        ValidateDebitCredit();
        Status = VoucherStatus.Posted;
        PostedByUserId = userId;
        PostedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates that total debit equals total credit and updates totals. Raises JournalBalancedEvent.
    /// </summary>
    public void ValidateDebitCredit()
    {
        if (JournalLines == null || JournalLines.Count == 0) 
            throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Journal lines cannot be null or empty.");
        
        decimal debits = 0;
        decimal credits = 0;

        foreach (var line in JournalLines)
        {
            debits += line.DebitAmount ?? 0;
            credits += line.CreditAmount ?? 0;
        }

        if (debits != credits)
            throw new ErpShowroom.Domain.Common.Exceptions.DomainException($"Journal is not balanced. Debits: {debits}, Credits: {credits}");

        TotalDebit = debits;
        TotalCredit = credits;

        AddDomainEvent(new ErpShowroom.Domain.acc.Events.JournalBalancedEvent(Id, debits, DateTime.UtcNow));
    }
}

public class JournalLine : BaseEntity
{
    public long? JournalId { get; set; }
    public long? AccountId { get; set; }
    public decimal? DebitAmount { get; set; } = 0;
    public decimal? CreditAmount { get; set; } = 0;
    public int? CostCenterId { get; set; }
    public string? Description { get; set; }

    [ForeignKey(nameof(JournalId))]
    public virtual JournalEntry? Journal { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual ChartOfAccount? Account { get; set; }
}

public class TrialBalance : BaseEntity
{
    public long? PeriodId { get; set; }
    public long? AccountId { get; set; }
    public decimal? OpeningDebit { get; set; }
    public decimal? OpeningCredit { get; set; }
    public decimal? MovementDebit { get; set; }
    public decimal? MovementCredit { get; set; }
    public decimal? ClosingDebit { get; set; }
    public decimal? ClosingCredit { get; set; }
    public DateTime? CalculatedAt { get; set; } = DateTime.UtcNow;
}

// ==== ADDITIONAL TABLES (ACC) ====
public class BranchCashClosing : BaseEntity
{
    public DateTime? ClosingDate { get; set; }
    public decimal? OpeningCash { get; set; }
    public decimal? TotalCollection { get; set; }
    public decimal? TotalExpense { get; set; }
    public decimal? TotalDeposit { get; set; }
    public decimal? ClosingCash { get; set; }
    public decimal? SystemClosingCash { get; set; }
    public string? Remarks { get; set; }
    public long? ClosedByUserId { get; set; }
    public DateTime? ClosedAt { get; set; } = DateTime.UtcNow;
}

public class FixedAsset : BaseEntity
{
    [Required, MaxLength(50)] public string? AssetCode { get; set; }
    [Required, MaxLength(200)] public string? AssetName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchaseCost { get; set; }
    public decimal? SalvageValue { get; set; }
    public int? UsefulLifeYears { get; set; }
    public string? DepreciationMethod { get; set; } = "StraightLine";
    public decimal? CurrentBookValue { get; set; }
    public long? BranchId { get; set; }
    public string? Location { get; set; }
    public string? AssetTag { get; set; }
    public bool? IsActive { get; set; } = true;
}

public class DepreciationSchedule : BaseEntity
{
    public long? AssetId { get; set; }
    public DateTime? DepreciationDate { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public decimal? DepreciationAmount { get; set; }
    public decimal? AccumulatedDepreciation { get; set; }
    public decimal? BookValueAfter { get; set; }
    public bool? IsPosted { get; set; } = false;
    public long? JournalId { get; set; }

    [ForeignKey(nameof(AssetId))]
    public virtual FixedAsset? Asset { get; set; }
}
