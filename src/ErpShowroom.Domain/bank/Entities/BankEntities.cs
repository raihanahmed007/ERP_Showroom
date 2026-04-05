using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.bank.Entities;

public class BankAccount : BaseEntity
{
    [Required, MaxLength(100)] public string? AccountName { get; set; }
    [Required, MaxLength(50)] public string? AccountNumber { get; set; }
    [MaxLength(100)] public string? BankName { get; set; }
    [MaxLength(100)] public string? BranchName { get; set; }
    public string? RoutingNumber { get; set; }
    public string? SwiftCode { get; set; }
    public decimal? CurrentBalance { get; set; }
    public long? GLAccountId { get; set; }
}

public class BankStatement : BaseEntity
{
    public long? BankAccountId { get; set; }
    public DateTime? StatementDate { get; set; }
    public decimal? OpeningBalance { get; set; }
    public decimal? ClosingBalance { get; set; }
    public string? UploadedFilePath { get; set; }
    public bool? IsReconciled { get; set; } = false;
    public DateTime? ReconciledAt { get; set; }
    public long? ReconciledByUserId { get; set; }

    [ForeignKey(nameof(BankAccountId))]
    public virtual BankAccount? BankAccount { get; set; }

    public virtual ICollection<StatementLine>? Lines { get; set; }
}

public class StatementLine : BaseEntity
{
    public long? StatementId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? Description { get; set; }
    public decimal? Debit { get; set; } = 0;
    public decimal? Credit { get; set; } = 0;
    public long? MatchedWithPaymentId { get; set; }
    public string? ReferenceNo { get; set; }

    [ForeignKey(nameof(StatementId))]
    public virtual BankStatement? Statement { get; set; }
}

public class CashDeposit : BaseEntity
{
    public DateTime? DepositDate { get; set; }
    public string? DepositSlipNo { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? UploadedImagePath { get; set; }
    public decimal? ReconciledAmount { get; set; }
    public long? ReconciledByUserId { get; set; }
    public DateTime? ReconciledAt { get; set; }
    public long? BankAccountId { get; set; }

    [ForeignKey(nameof(BankAccountId))]
    public virtual BankAccount? BankAccount { get; set; }
}
