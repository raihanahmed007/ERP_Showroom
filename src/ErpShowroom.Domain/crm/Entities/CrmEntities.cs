using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.doc.Entities;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Domain.crm.Entities;

public class Customer : BaseEntity
{
    [Required, MaxLength(50)] public string? CustomerCode { get; set; }
    [Required, MaxLength(200)] public string? FullName { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? SpouseName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NIDNumber { get; set; }
    public string? TINNumber { get; set; }
    [Required, MaxLength(20)] public string? Phone { get; set; }
    public string? AlternatePhone { get; set; }
    public string? Email { get; set; }
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public string? Occupation { get; set; }
    public decimal? MonthlyIncome { get; set; }
    public string? EmployerName { get; set; }
    public byte[]? ProfilePhoto { get; set; }
    public long? ReferenceCustomerId { get; set; }
    public string? CustomerType { get; set; } = "Individual";
    public string? CompanyName { get; set; }
    public string? TradeLicenseNo { get; set; }
    public string? EncryptedNid { get; set; }
    public string? EncryptedPhone { get; set; }

    [InverseProperty(nameof(HPAgreement.Customer))]
    public virtual ICollection<HPAgreement>? HPAgreements { get; set; }

    public virtual ICollection<StoredDocument>? Documents { get; set; }
    public virtual ICollection<AISentimentLog>? Sentiments { get; set; }
    public virtual ICollection<FollowupTask>? FollowupTasks { get; set; }
}

public class Lead : BaseEntity
{
    public LeadSourceEnum? LeadSource { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public long? InterestedProductId { get; set; }
    public LeadStatusEnum? LeadStatus { get; set; } = LeadStatusEnum.New;
    public long? AssignedToUserId { get; set; }
    public long? ConvertedToCustomerId { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public string? Notes { get; set; }
}

public class AISentimentLog : BaseEntity
{
    public long? CustomerId { get; set; }
    public string? ConversationText { get; set; }
    public decimal? SentimentScore { get; set; }
    public string? SentimentLabel { get; set; }
    public string? SuggestedAction { get; set; }
    public string? Emotion { get; set; }
    public string? IntentDetected { get; set; }
    public DateTime? ConversationDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }
}

public class FollowupTask : BaseEntity
{
    public long? CustomerId { get; set; }
    public string? TaskType { get; set; } // Call, SMS, Visit, Email
    public DateTime? DueDate { get; set; }
    public long? AssignedToUserId { get; set; }
    public TaskStatusEnum? Status { get; set; } = TaskStatusEnum.Pending;
    public string? CompletionNotes { get; set; }
    public bool? CreatedByAI { get; set; } = false;
    public string? Priority { get; set; } = "Medium";
    public DateTime? CompletedAt { get; set; }
    public string? FollowupResult { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }
}

public class Campaign : BaseEntity
{
    public string? CampaignName { get; set; }
    public string? CampaignType { get; set; } // Birthday, Exchange, Warranty, Festival
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public string? MessageTemplate { get; set; }
    public bool? IsActive { get; set; } = true;
    public long? TargetSegmentId { get; set; }
}
