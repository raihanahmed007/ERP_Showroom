using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.inv.Entities;

namespace ErpShowroom.Domain.fin.Entities;

public class HPAgreement : BaseEntity
{
    [Required, MaxLength(50)] public string? AgreementNo { get; set; }
    public long? CustomerId { get; set; }
    public long? ProductId { get; set; }
    public DateTime? AgreementDate { get; set; }
    public decimal? ProductPrice { get; set; }
    public decimal? DownPayment { get; set; }
    public decimal? FinanceAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? TotalPayable { get; set; }
    public int? InstallmentCount { get; set; }
    public decimal? InstallmentAmount { get; set; }
    public HPAgreementStatus? Status { get; set; } = HPAgreementStatus.PendingApproval;
    public long? GuarantorId { get; set; }
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? RejectionReason { get; set; }
    public long? WrittenOffByUserId { get; set; }
    public DateTime? WrittenOffAt { get; set; }
    public decimal? WrittenOffAmount { get; set; }
    public long? CollectionOfficerId { get; set; }
    public string? SalesPersonCode { get; set; }
    public decimal? LatePaymentPenaltyRate { get; set; } = 2.0m;
    public long? ApprovalWorkflowId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }

    public virtual ICollection<EMISchedule>? EMISchedules { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual RecoveryBoard? RecoveryBoard { get; set; }

    /// <summary>
    /// Calculates the finance details and installment amount.
    /// Formula: InstallmentAmount = FinanceAmount * (r * (1+r)^n) / ((1+r)^n - 1)
    /// where r = monthly rate, n = InstallmentCount
    /// </summary>
    public void CalculateEMI()
    {
        FinanceAmount = (ProductPrice ?? 0) - (DownPayment ?? 0);
        int n = InstallmentCount ?? 1;
        
        if (InterestRate > 0)
        {
            decimal r = InterestRate.Value / 100m / 12m;
            double factor = Math.Pow(1 + (double)r, n);
            InstallmentAmount = FinanceAmount * (decimal)((double)r * factor / (factor - 1));
        }
        else
        {
            InstallmentAmount = FinanceAmount / n;
        }

        TotalPayable = Math.Round(InstallmentAmount.Value * n, 2);
        InstallmentAmount = Math.Round(InstallmentAmount.Value, 2);
    }
}

public class EMISchedule : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public int? InstallmentNo { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? PrincipalAmount { get; set; }
    public decimal? InterestAmount { get; set; }
    public decimal? TotalDue { get; set; }
    public decimal? PaidAmount { get; set; } = 0;
    public decimal? PenaltyAmount { get; set; } = 0;
    public EMIPaymentStatus? Status { get; set; } = EMIPaymentStatus.Due;
    public DateTime? PaidDate { get; set; }
    public DateTime? LastReminderSentAt { get; set; }
    public int? LastReminderType { get; set; }
    public bool? IsLegalNoticeSent { get; set; } = false;

    [ForeignKey(nameof(HPAgreementId))]
    public virtual HPAgreement? Agreement { get; set; }

    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual ICollection<Penalty>? Penalties { get; set; }

    /// <summary>
    /// Calculates the late payment penalty and records it.
    /// Formula: PenaltyAmount = TotalDue * (penaltyRate / 100) * daysOverdue
    /// </summary>
    public decimal CalculatePenalty(int daysOverdue, decimal penaltyRate)
    {
        if (daysOverdue <= 0) return 0;
        
        decimal amount = (TotalDue ?? 0) * (penaltyRate / 100m) * daysOverdue;
        if (amount > 0)
        {
            if (Penalties == null) Penalties = new List<Penalty>();
            Penalties.Add(new Penalty
            {
                EMIId = Id,
                DaysOverdue = daysOverdue,
                PenaltyRate = penaltyRate,
                PenaltyAmount = amount,
                PenaltyDate = DateTime.UtcNow
            });
            PenaltyAmount = (PenaltyAmount ?? 0) + amount;
        }
        return amount;
    }
}

public class Payment : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public long? EMIId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public decimal? Amount { get; set; }
    public PaymentMethodEnum? PaymentMethod { get; set; }
    [MaxLength(100)] public string? ReferenceNo { get; set; }
    public long? CollectorEmployeeId { get; set; }
    public bool? IsPartial { get; set; } = false;
    public string? Notes { get; set; }
    public long? BankAccountId { get; set; }
    public string? SlipImagePath { get; set; }
    public bool? IsReconciledWithBank { get; set; } = false;
    public DateTime? ReconciledAt { get; set; }
    public long? ReconciledByUserId { get; set; }

    [ForeignKey(nameof(EMIId))]
    public virtual EMISchedule? EMI { get; set; }

    [ForeignKey(nameof(HPAgreementId))]
    public virtual HPAgreement? Agreement { get; set; }
}

public class Penalty : BaseEntity
{
    public long? EMIId { get; set; }
    public DateTime? PenaltyDate { get; set; }
    public int? DaysOverdue { get; set; }
    public decimal? PenaltyRate { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public bool? IsWaived { get; set; } = false;
    public long? WaivedByUserId { get; set; }
    public string? WaivedReason { get; set; }
    public bool? IsPaid { get; set; } = false;
    public long? PaidAgainstPaymentId { get; set; }

    [ForeignKey(nameof(EMIId))]
    public virtual EMISchedule? EMI { get; set; }
}

public class RecoveryBoard : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public RiskBucketEnum? RiskBucket { get; set; }
    public decimal? TotalOverdue { get; set; }
    public DateTime? LastFollowupDate { get; set; }
    public long? AssignedCollectorId { get; set; }
    public DateTime? PromiseToPayDate { get; set; }
    public bool? LegalEscalationFlag { get; set; } = false;
    public long? LegalCaseId { get; set; }
    public string? CollectorNotes { get; set; }
    public int? RecoveryScore { get; set; }
    public DateTime? NextFollowupDate { get; set; }
    public string? AiSuggestedAction { get; set; }
    public decimal? AiConfidenceScore { get; set; }

    [ForeignKey(nameof(HPAgreementId))]
    public virtual HPAgreement? Agreement { get; set; }
}

public class Guarantor : BaseEntity
{
    public long? CustomerId { get; set; }
    [Required, MaxLength(200)] public string? GuarantorName { get; set; }
    [MaxLength(50)] public string? NIDNumber { get; set; }
    [MaxLength(20)] public string? Phone { get; set; }
    [MaxLength(100)] public string? Email { get; set; }
    [MaxLength(500)] public string? Address { get; set; }
    [MaxLength(50)] public string? Relationship { get; set; }
    public long? SignedAgreementDocId { get; set; }
    public decimal? MonthlyIncome { get; set; }
    public string? EmployerName { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }
}

// ==== ADDITIONAL TABLES (FIN) ====
public class SalesInvoice : BaseEntity
{
    [Required, MaxLength(50)] public string? InvoiceNo { get; set; }
    public long? CustomerId { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public long? HPAgreementId { get; set; }
    public string? PaymentStatus { get; set; } = "Unpaid";
    public long? CreatedByUserId { get; set; }

    public virtual ICollection<SalesInvoiceDetail>? Details { get; set; }
}

public class SalesInvoiceDetail : BaseEntity
{
    public long? InvoiceId { get; set; }
    public long? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public long? SerialId { get; set; }

    [ForeignKey(nameof(InvoiceId))]
    public virtual SalesInvoice? Invoice { get; set; }
}

public class LegalNotice : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public string? NoticeNo { get; set; }
    public DateTime? NoticeDate { get; set; }
    public DateTime? ResponseDeadline { get; set; }
    public string? NoticeType { get; set; }
    public string? SentVia { get; set; }
    public string? SentToAddress { get; set; }
    public string? DocumentPath { get; set; }
    public bool? IsResponded { get; set; } = false;
    public DateTime? ResponseDate { get; set; }
    public string? ResponseNotes { get; set; }
}

public class RepossessionCase : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public string? CaseNo { get; set; }
    public DateTime? FilingDate { get; set; }
    public string? CourtName { get; set; }
    public string? CurrentStage { get; set; }
    public DateTime? RepossessionDate { get; set; }
    public string? RepossessionNote { get; set; }
    public decimal? LegalCost { get; set; }
    public bool? IsClosed { get; set; } = false;
}

public class VehicleRegistration : BaseEntity
{
    public string? VehicleVIN { get; set; }
    public string? RegistrationNo { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string? EngineNo { get; set; }
    public string? ChassisNo { get; set; }
    public long? CustomerId { get; set; }
    public long? HPAgreementId { get; set; }
    public string? RegistrationDocumentPath { get; set; }
}

public class InsurancePolicy : BaseEntity
{
    public long? HPAgreementId { get; set; }
    public string? PolicyNo { get; set; }
    public string? InsuranceCompany { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? PremiumAmount { get; set; }
    public decimal? CoverageAmount { get; set; }
    public string? PolicyDocumentPath { get; set; }
    public bool? IsClaimed { get; set; } = false;
}
