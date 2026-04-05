using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.sys.Entities;

public class User : BaseEntity
{
    [Required, MaxLength(100)] public string? UserName { get; set; }
    [MaxLength(100)] public string? NormalizedUserName { get; set; }
    [Required] public string? PasswordHash { get; set; }
    [MaxLength(256)] public string? Email { get; set; }
    [MaxLength(20)] public string? Phone { get; set; }
    public bool? EmailConfirmed { get; set; } = false;
    public bool? PhoneConfirmed { get; set; } = false;
    public bool? TwoFactorEnabled { get; set; } = false;
    public string? SecurityStamp { get; set; }
    public int? FailedLoginCount { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; }
    public long? EmployeeId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<UserRole>? Roles { get; set; }
}

public class Role : BaseEntity
{
    [Required, MaxLength(50)] public string? Name { get; set; }
    [MaxLength(50)] public string? NormalizedName { get; set; }
    [MaxLength(200)] public string? Description { get; set; }
}

public class Permission : BaseEntity
{
    [Required, MaxLength(50)] public string? ModuleName { get; set; }
    [Required, MaxLength(50)] public string? ActionName { get; set; }
    [Required, MaxLength(100)] public string? PermissionKey { get; set; }
}

public class UserRole : BaseEntity
{
    public long? UserId { get; set; }
    public long? RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
}

public class RolePermission : BaseEntity
{
    public long? RoleId { get; set; }
    public long? PermissionId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }

    [ForeignKey(nameof(PermissionId))]
    public virtual Permission? Permission { get; set; }
}

public class AuditLog : BaseEntity
{
    [Required, MaxLength(100)] public string? TableName { get; set; }
    [Required, MaxLength(100)] public string? RecordId { get; set; }
    public string? Action { get; set; } // INSERT, UPDATE, DELETE, SOFT_DELETE
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public long? ChangedByUserId { get; set; }
    public DateTime? ChangedAt { get; set; } = DateTime.UtcNow;
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
}

public class SoftDeleteRegistry : BaseEntity
{
    public string? TableName { get; set; }
    public string? RecordId { get; set; }
    public long? DeletedByUserId { get; set; }
    public DateTime? DeletedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RestoredAt { get; set; }
    public long? RestoredByUserId { get; set; }
}

public class Branch : BaseEntity
{
    [Required, MaxLength(100)] public string? BranchName { get; set; }
    [Required, MaxLength(20)] public string? BranchCode { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool? IsHeadOffice { get; set; } = false;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? OpeningTime { get; set; }
    public string? ClosingTime { get; set; }
}

public class Notification : BaseEntity
{
    public long? UserId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public bool? IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public string? NotificationType { get; set; } // Alert, Reminder, Approval
    public string? RelatedEntity { get; set; }
    public long? RelatedEntityId { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}

// ==== ADDITIONAL TABLES (SYS / MESSAGING / BI) ====
public class SmsQueue : BaseEntity
{
    public string? PhoneNumber { get; set; }
    public string? MessageText { get; set; }
    public string? SenderId { get; set; }
    public DateTime? ScheduleTime { get; set; }
    public bool? IsSent { get; set; } = false;
    public DateTime? SentAt { get; set; }
    public string? ResponseJson { get; set; }
    public int? RetryCount { get; set; } = 0;
}

public class WhatsAppQueue : BaseEntity
{
    public string? PhoneNumber { get; set; }
    public string? MessageText { get; set; }
    public string? MediaUrl { get; set; }
    public DateTime? ScheduleTime { get; set; }
    public bool? IsSent { get; set; } = false;
    public DateTime? SentAt { get; set; }
    public string? ResponseJson { get; set; }
}

public class BISnapshot : BaseEntity
{
    public DateTime? SnapshotDate { get; set; }
    public long? BranchId { get; set; }
    public decimal? TotalSales { get; set; }
    public decimal? TotalCollection { get; set; }
    public decimal? TotalOverdue { get; set; }
    public decimal? TotalStockValue { get; set; }
    public int? ActiveHPAgreements { get; set; }
    public decimal? RecoveryRate { get; set; }
    public string? AdditionalMetricsJson { get; set; }
}

public class ForecastSnapshot : BaseEntity
{
    public DateTime? ForecastDate { get; set; }
    public long? BranchId { get; set; }
    public long? ProductId { get; set; }
    public int? ForecastedQuantity { get; set; }
    public decimal? ForecastedRevenue { get; set; }
    public decimal? ConfidenceLower { get; set; }
    public decimal? ConfidenceUpper { get; set; }
    public string? ModelVersion { get; set; }
    public DateTime? GeneratedAt { get; set; } = DateTime.UtcNow;
}

public class DataWarehouseSyncLog : BaseEntity
{
    public DateTime? SyncStartTime { get; set; }
    public DateTime? SyncEndTime { get; set; }
    public string? SyncType { get; set; }
    public long? RecordsSynced { get; set; }
    public bool? IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}

public class VectorEmbedding : BaseEntity
{
    public string? EntityType { get; set; }
    public long? EntityId { get; set; }
    public string? EmbeddingVector { get; set; }
    public string? EmbeddingModel { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PromptTemplate : BaseEntity
{
    [Required, MaxLength(100)] public string? Name { get; set; }
    public string? TemplateText { get; set; }
    public string? Version { get; set; }
    public bool? IsActive { get; set; } = true;
    public string? Description { get; set; }
}
