using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.inv.Entities;

namespace ErpShowroom.Domain.wrk.Entities;

public class JobCard : BaseEntity
{
    [Required, MaxLength(50)] public string? JobCardNo { get; set; }
    public long? CustomerId { get; set; }
    public string? VehicleVIN { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public int? ReceivedOdometer { get; set; }
    public string? CustomerComplaint { get; set; }
    public JobCardStatus? Status { get; set; } = JobCardStatus.Pending;
    public long? AssignedTechnicianId { get; set; }
    public DateTime? CompletedDate { get; set; }
    public decimal? TotalCost { get; set; }
    public decimal? PaidAmount { get; set; }
    public string? PaymentStatus { get; set; } = "Unpaid";
    public DateTime? DeliveryDate { get; set; }
    public string? DeliverySignaturePath { get; set; }
    public string? ServiceAdvisorName { get; set; }

    public virtual ICollection<JobService>? Services { get; set; }
    public virtual ICollection<SparePartUsed>? SpareParts { get; set; }
}

public class JobService : BaseEntity
{
    public long? JobCardId { get; set; }
    public string? ServiceName { get; set; }
    public int? StandardTimeMinutes { get; set; }
    public int? ActualTimeMinutes { get; set; }
    public decimal? LaborCharge { get; set; }
    public string? TechnicianRemarks { get; set; }

    [ForeignKey(nameof(JobCardId))]
    public virtual JobCard? JobCard { get; set; }
}

public class SparePartUsed : BaseEntity
{
    public long? JobCardId { get; set; }
    public long? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? SerialNoUsed { get; set; }
    public bool? IsWarrantyClaim { get; set; } = false;

    [ForeignKey(nameof(JobCardId))]
    public virtual JobCard? JobCard { get; set; }
}

public class TechnicianEfficiency : BaseEntity
{
    public long? TechnicianId { get; set; }
    public DateTime? MonthYear { get; set; }
    public decimal? TotalStandardHours { get; set; }
    public decimal? TotalActualHours { get; set; }
    public int? JobCardsCompleted { get; set; }
    public decimal? CustomerRating { get; set; }
}

public class WarrantyClaim : BaseEntity
{
    public long? ProductId { get; set; }
    public string? SerialNo { get; set; }
    public long? CustomerId { get; set; }
    public DateTime? ClaimDate { get; set; }
    public string? IssueDescription { get; set; }
    public string? Status { get; set; } = "Pending";
    public DateTime? ResolutionDate { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? ApprovedByUserId { get; set; }
}
