using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.sys.Entities;

public enum CompanyType { Main, Sub }
public enum FeatureType { Page, API, Report, Button }
public enum PageType { List, Form, Report, Dashboard }
public enum RoleType { SystemAdmin, CompanyAdmin, User, ReadOnly }
public enum OutputFormat { Grid, PDF, Excel, Chart }
public enum ConfigDataType { String, Int, Bool, Json, DateTime }
public enum BackupType { Full, Differential, Log }
public enum BackupStatus { Running, Success, Failed }
public enum AuditAction { Insert, Update, Delete, Login, Logout, Export, Permission }

[Table("Companies", Schema = "sys")]
public class Company : BaseEntity
{
    [Required] public string CompanyName { get; set; } = string.Empty;
    [Required] public string CompanyCode { get; set; } = string.Empty;
    public CompanyType CompanyType { get; set; }
    public long? ParentCompanyId { get; set; }
    [ForeignKey("ParentCompanyId")] public Company? ParentCompany { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? LogoUrl { get; set; }
    public string? LicenseNo { get; set; }
    public string? TaxId { get; set; }
    public string? ContactPerson { get; set; }
    public bool IsHeadOffice { get; set; }
    public DateTime? ActivationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int MaxUsers { get; set; } = 50;
    public string? DatabaseName { get; set; }
}
[Table("Modules", Schema = "sys")]
public class AppModule : BaseEntity
{
    [Required] public string ModuleName { get; set; } = string.Empty;
    [Required] public string ModuleCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public int SortOrder { get; set; }
    public long? ParentModuleId { get; set; }
    [ForeignKey("ParentModuleId")] public AppModule? ParentModule { get; set; }
    public string? RoutePrefix { get; set; }
}
[Table("Features", Schema = "sys")]
public class Feature : BaseEntity
{
    [Required] public string FeatureName { get; set; } = string.Empty;
    [Required] public string FeatureCode { get; set; } = string.Empty;
    public long ModuleId { get; set; }
    [ForeignKey("ModuleId")] public AppModule Module { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public FeatureType FeatureType { get; set; }
}

[Table("Pages", Schema = "sys")]
public class AppPage : BaseEntity
{
    [Required] public string PageName { get; set; } = string.Empty;
    [Required] public string PageCode { get; set; } = string.Empty;
    public long FeatureId { get; set; }
    [ForeignKey("FeatureId")] public Feature Feature { get; set; } = null!;
    [Required] public string RouteUrl { get; set; } = string.Empty;
    public string? ComponentName { get; set; }
    public string? IconName { get; set; }
    public int SortOrder { get; set; }
    public bool RequiresApproval { get; set; }
    public PageType PageType { get; set; }
}

[Table("Users", Schema = "sys")]
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

[Table("Roles", Schema = "sys")]
public class Role : BaseEntity
{
    [Required] public string RoleName { get; set; } = string.Empty;
    [Required] public string RoleCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleType RoleType { get; set; }
    public long? ParentCompanyId { get; set; }
    [ForeignKey("ParentCompanyId")] public Company? Company { get; set; }

    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
}

[Table("Permissions", Schema = "sys")]
public class Permission : BaseEntity
{
    [Required, MaxLength(50)] public string? ModuleName { get; set; }
    [Required, MaxLength(50)] public string? ActionName { get; set; }
    [Required, MaxLength(100)] public string? PermissionKey { get; set; }
}
[Table("CompanyModuleAccess", Schema = "sys")]
public class CompanyModuleAccess : BaseEntity
{
    public long CompanyId { get; set; }
    [ForeignKey("CompanyId")] public Company Company { get; set; } = null!;
    public long ModuleId { get; set; }
    [ForeignKey("ModuleId")] public AppModule Module { get; set; } = null!;
    public bool IsEnabled { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public long GrantedBy { get; set; }
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}

[Table("PagePermissions", Schema = "sys")]
public class PagePermission : BaseEntity
{
    public long RoleId { get; set; }
    [ForeignKey("RoleId")] public Role Role { get; set; } = null!;
    public long PageId { get; set; }
    [ForeignKey("PageId")] public AppPage Page { get; set; } = null!;
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
}

[Table("ApprovalSetups", Schema = "sys")]
public class ApprovalSetup : BaseEntity
{
    public long CompanyId { get; set; }
    public long PageId { get; set; }
    [ForeignKey("PageId")] public AppPage Page { get; set; } = null!;
    public int StepOrder { get; set; }
    public long? ApproverRoleId { get; set; }
    public long? ApproverUserId { get; set; }
    public bool IsParallel { get; set; }
    public int TimeoutHours { get; set; }
    public long? EscalationRoleId { get; set; }
}


[Table("UserPageAccess", Schema = "sys")]
public class UserPageAccess : BaseEntity
{
    public long UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; } = null!;
    public long PageId { get; set; }
    [ForeignKey("PageId")] public AppPage Page { get; set; } = null!;
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
    public bool OverridesRole { get; set; }
}
[Table("UserRoles", Schema = "sys")]
public class UserRole : BaseEntity
{
    public long? UserId { get; set; }
    public long? RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
}

[Table("RolePermissions", Schema = "sys")]
public class RolePermission : BaseEntity
{
    public long? RoleId { get; set; }
    public long? PermissionId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }

    [ForeignKey(nameof(PermissionId))]
    public virtual Permission? Permission { get; set; }
}

[Table("ReportTypes", Schema = "sys")]
public class ReportType : BaseEntity
{
    [Required] public string ReportTypeName { get; set; } = string.Empty;
    [Required] public string ReportTypeCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long ModuleId { get; set; }
    [ForeignKey("ModuleId")] public AppModule Module { get; set; } = null!;
    public int SortOrder { get; set; }
}

[Table("ReportNames", Schema = "sys")]
public class ReportName : BaseEntity
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string ReportCode { get; set; } = string.Empty;
    public long ReportTypeId { get; set; }
    [ForeignKey("ReportTypeId")] public ReportType ReportType { get; set; } = null!;
    public string? Description { get; set; }
    public string? ReportQuery { get; set; }
    public int SortOrder { get; set; }
    public OutputFormat OutputFormat { get; set; }
    public string? Parameters { get; set; }
}

[Table("UserDashboardAccess", Schema = "sys")]
public class UserDashboardAccess : BaseEntity
{
    public long UserId { get; set; }
    public long CompanyId { get; set; }
    public string? DashboardWidgets { get; set; }
    public string? LayoutConfig { get; set; }
    public bool IsCustomized { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}

[Table("AuditLogs", Schema = "sys")]
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

[Table("DatabaseBackupLogs", Schema = "sys")]
public class DatabaseBackupLog : BaseEntity
{
    public string? BackupFileName { get; set; }
    public string? BackupPath { get; set; }
    public long BackupSizeKB { get; set; }
    public BackupType BackupType { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public BackupStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public int TriggeredBy { get; set; }
    public bool IsAutomatic { get; set; }
    public int DownloadCount { get; set; }
}

[Table("UserLoginLogs", Schema = "sys")]
public class UserLoginLog : BaseEntity
{
    public long UserId { get; set; }
    public string? Username { get; set; }
    public DateTime LoginAt { get; set; }
    public DateTime? LogoutAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? OS { get; set; }
    public string? DeviceType { get; set; }
    public bool IsSuccess { get; set; }
    public string? FailureReason { get; set; }
    public int? SessionDuration { get; set; }
    public long? CompanyId { get; set; }
}
