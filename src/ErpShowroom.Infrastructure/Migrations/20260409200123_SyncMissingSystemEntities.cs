using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpShowroom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncMissingSystemEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppModule",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ParentModuleId = table.Column<long>(type: "bigint", nullable: true),
                    RoutePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppModule_AppModule_ParentModuleId",
                        column: x => x.ParentModuleId,
                        principalSchema: "erp_sys",
                        principalTable: "AppModule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DatabaseBackupLog",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackupFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupSizeKB = table.Column<long>(type: "bigint", nullable: false),
                    BackupType = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TriggeredBy = table.Column<int>(type: "int", nullable: false),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseBackupLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftDeleteRegistry",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftDeleteRegistry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDashboardAccess",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DashboardWidgets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LayoutConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCustomized = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDashboardAccess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLog",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionDuration = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyModuleAccess",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrantedBy = table.Column<long>(type: "bigint", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyModuleAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyModuleAccess_AppModule_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "erp_sys",
                        principalTable: "AppModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyModuleAccess_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "erp_sys",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeatureCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    FeatureType = table.Column<int>(type: "int", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feature_AppModule_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "erp_sys",
                        principalTable: "AppModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportType",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportType_AppModule_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "erp_sys",
                        principalTable: "AppModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPage",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeatureId = table.Column<long>(type: "bigint", nullable: false),
                    RouteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    PageType = table.Column<int>(type: "int", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPage_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "erp_sys",
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportName",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportQuery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    OutputFormat = table.Column<int>(type: "int", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportName_ReportType_ReportTypeId",
                        column: x => x.ReportTypeId,
                        principalSchema: "erp_sys",
                        principalTable: "ReportType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalSetup",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    PageId = table.Column<long>(type: "bigint", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    ApproverRoleId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsParallel = table.Column<bool>(type: "bit", nullable: false),
                    TimeoutHours = table.Column<int>(type: "int", nullable: false),
                    EscalationRoleId = table.Column<long>(type: "bigint", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalSetup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalSetup_AppPage_PageId",
                        column: x => x.PageId,
                        principalSchema: "erp_sys",
                        principalTable: "AppPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagePermission",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PageId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanApprove = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagePermission_AppPage_PageId",
                        column: x => x.PageId,
                        principalSchema: "erp_sys",
                        principalTable: "AppPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "erp_sys",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPageAccess",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PageId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanApprove = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false),
                    OverridesRole = table.Column<bool>(type: "bit", nullable: false),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFromIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPageAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPageAccess_AppPage_PageId",
                        column: x => x.PageId,
                        principalSchema: "erp_sys",
                        principalTable: "AppPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPageAccess_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "erp_sys",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppModule_ParentModuleId",
                schema: "erp_sys",
                table: "AppModule",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPage_FeatureId",
                schema: "erp_sys",
                table: "AppPage",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSetup_PageId",
                schema: "erp_sys",
                table: "ApprovalSetup",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModuleAccess_CompanyId",
                schema: "erp_sys",
                table: "CompanyModuleAccess",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModuleAccess_ModuleId",
                schema: "erp_sys",
                table: "CompanyModuleAccess",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_ModuleId",
                schema: "erp_sys",
                table: "Feature",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_PageId",
                schema: "erp_sys",
                table: "PagePermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_RoleId",
                schema: "erp_sys",
                table: "PagePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportName_ReportTypeId",
                schema: "erp_sys",
                table: "ReportName",
                column: "ReportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportType_ModuleId",
                schema: "erp_sys",
                table: "ReportType",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPageAccess_PageId",
                schema: "erp_sys",
                table: "UserPageAccess",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPageAccess_UserId",
                schema: "erp_sys",
                table: "UserPageAccess",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalSetup",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "CompanyModuleAccess",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "DatabaseBackupLog",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "PagePermission",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "ReportName",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "SoftDeleteRegistry",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "UserDashboardAccess",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "UserLoginLog",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "UserPageAccess",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "ReportType",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "AppPage",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "Feature",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "AppModule",
                schema: "erp_sys");
        }
    }
}
