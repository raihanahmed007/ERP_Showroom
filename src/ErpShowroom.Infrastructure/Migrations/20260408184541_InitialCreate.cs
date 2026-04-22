using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpShowroom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.EnsureSchema(
                name: "wf");

            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "erp_sys");

            migrationBuilder.EnsureSchema(
                name: "bank");

            migrationBuilder.EnsureSchema(
                name: "acc");

            migrationBuilder.EnsureSchema(
                name: "inv");

            migrationBuilder.EnsureSchema(
                name: "doc");

            migrationBuilder.EnsureSchema(
                name: "fin");

            migrationBuilder.EnsureSchema(
                name: "prc");

            migrationBuilder.EnsureSchema(
                name: "prl");

            migrationBuilder.EnsureSchema(
                name: "wrk");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SwiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GLAccountId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BISnapshot",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCollection = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalOverdue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalStockValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActiveHPAgreements = table.Column<int>(type: "int", nullable: true),
                    RecoveryRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalMetricsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_BISnapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHeadOffice = table.Column<bool>(type: "bit", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpeningTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosingTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchCashClosing",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningCash = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCollection = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalExpense = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosingCash = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SystemClosingCash = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchCashClosing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                schema: "crm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampaignType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    TargetSegmentId = table.Column<long>(type: "bigint", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChartOfAccount",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: true),
                    ParentAccountId = table.Column<long>(type: "bigint", nullable: true),
                    IsHead = table.Column<bool>(type: "bit", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ChartOfAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartOfAccount_ChartOfAccount_ParentAccountId",
                        column: x => x.ParentAccountId,
                        principalSchema: "acc",
                        principalTable: "ChartOfAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyType = table.Column<int>(type: "int", nullable: false),
                    ParentCompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHeadOffice = table.Column<bool>(type: "bit", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    DatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Company_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalSchema: "erp_sys",
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "crm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TINNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlternatePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePhoto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ReferenceCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeLicenseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptedNid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptedPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataWarehouseSyncLog",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SyncStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyncEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyncType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordsSynced = table.Column<long>(type: "bigint", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DataWarehouseSyncLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCategory",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RetentionYears = table.Column<int>(type: "int", nullable: true),
                    RequiresLegalHold = table.Column<bool>(type: "bit", nullable: true),
                    AllowedMimeTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxFileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_DocumentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PFNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportingToEmployeeId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriod",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    ClosedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextPeriodId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_FiscalPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixedAsset",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalvageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsefulLifeYears = table.Column<int>(type: "int", nullable: true),
                    DepreciationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentBookValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_FixedAsset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForecastSnapshot",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForecastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ForecastedQuantity = table.Column<int>(type: "int", nullable: true),
                    ForecastedRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConfidenceLower = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConfidenceUpper = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModelVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ForecastSnapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePolicy",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    PolicyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CoverageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PolicyDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsClaimed = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_InsurancePolicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCard",
                schema: "wrk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    VehicleVIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedOdometer = table.Column<int>(type: "int", nullable: true),
                    CustomerComplaint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AssignedTechnicianId = table.Column<long>(type: "bigint", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliverySignaturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceAdvisorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_JobCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lead",
                schema: "crm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadSource = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestedProductId = table.Column<long>(type: "bigint", nullable: true),
                    LeadStatus = table.Column<int>(type: "int", nullable: true),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: true),
                    ConvertedToCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ConvertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Lead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalNotice",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    NoticeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoticeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoticeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentVia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentToAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResponded = table.Column<bool>(type: "bit", nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LegalNotice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedEntity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedEntityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermissionKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_ProductCategory_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "inv",
                        principalTable: "ProductCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PromptTemplate",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TemplateText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_PromptTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepossessionCase",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    CaseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CourtName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepossessionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepossessionNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_RepossessionCase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoice",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_SalesInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsQueue",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_SmsQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockAging",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    DaysInStock = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_StockAging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfer",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromBranchId = table.Column<long>(type: "bigint", nullable: true),
                    ToBranchId = table.Column<long>(type: "bigint", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TINNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeadTimeDays = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianEfficiency",
                schema: "wrk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianId = table.Column<long>(type: "bigint", nullable: true),
                    MonthYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalStandardHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalActualHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JobCardsCompleted = table.Column<int>(type: "int", nullable: true),
                    CustomerRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_TechnicianEfficiency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrialBalance",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    OpeningDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpeningCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MovementDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MovementCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosingDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosingCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_TrialBalance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    PhoneConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: true),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VectorEmbedding",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    EmbeddingVector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbeddingModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_VectorEmbedding", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRegistration",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleVIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EngineNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChassisNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    RegistrationDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_VehicleRegistration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TINNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadTimeDays = table.Column<int>(type: "int", nullable: true),
                    AverageRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocation",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShelfNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WarehouseLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarrantyClaim",
                schema: "wrk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WarrantyClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppQueue",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WhatsAppQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDefinition",
                schema: "wf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    TimeoutHours = table.Column<int>(type: "int", nullable: true),
                    RowUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_WorkflowDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatement",
                schema: "bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
                    StatementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UploadedFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReconciled = table.Column<bool>(type: "bit", nullable: true),
                    ReconciledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReconciledByUserId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_BankStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankStatement_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "bank",
                        principalTable: "BankAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashDeposit",
                schema: "bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepositSlipNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UploadedImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReconciledAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReconciledByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ReconciledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_CashDeposit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashDeposit_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "bank",
                        principalTable: "BankAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    ParentCompanyId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Company_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalSchema: "erp_sys",
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AISentimentLog",
                schema: "crm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ConversationText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentimentScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SentimentLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuggestedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emotion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntentDetected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_AISentimentLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AISentimentLog_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FollowupTask",
                schema: "crm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CompletionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByAI = table.Column<bool>(type: "bit", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FollowupResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FollowupTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowupTask_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guarantor",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    GuarantorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NIDNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SignedAgreementDocId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Guarantor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guarantor_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoredDocument",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OCRText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsUnderLegalHold = table.Column<bool>(type: "bit", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    VerifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_StoredDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoredDocument_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoredDocument_DocumentCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "doc",
                        principalTable: "DocumentCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInLatitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckInLongitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: true),
                    LateMinutes = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelfieImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendance_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Incentive",
                schema: "prl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    IncentiveMonth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IncentiveType = table.Column<int>(type: "int", nullable: true),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AchievedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidInSalarySlipId = table.Column<long>(type: "bigint", nullable: true),
                    CalculationFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Incentive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incentive_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Leave",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveType = table.Column<int>(type: "int", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Leave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leave_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalance",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    LeaveType = table.Column<int>(type: "int", nullable: true),
                    TotalDays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsedDays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_LeaveBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveBalance_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalarySlip",
                schema: "prl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    MonthYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Basic = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedByUserId = table.Column<long>(type: "bigint", nullable: true),
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
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalarySlip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalarySlip_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalaryStructure",
                schema: "prl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Basic = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HouseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Medical = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProvidentFundPct = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxDeductionPct = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_SalaryStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryStructure_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JournalEntry",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: true),
                    JournalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodId = table.Column<long>(type: "bigint", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReversed = table.Column<bool>(type: "bit", nullable: true),
                    ReversedFromJournalId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_JournalEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntry_FiscalPeriod_PeriodId",
                        column: x => x.PeriodId,
                        principalSchema: "acc",
                        principalTable: "FiscalPeriod",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepreciationSchedule",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<long>(type: "bigint", nullable: true),
                    DepreciationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    DepreciationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AccumulatedDepreciation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BookValueAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPosted = table.Column<bool>(type: "bit", nullable: true),
                    JournalId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_DepreciationSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationSchedule_FixedAsset_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "acc",
                        principalTable: "FixedAsset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobService",
                schema: "wrk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    ActualTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    LaborCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TechnicianRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_JobService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobService_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalSchema: "wrk",
                        principalTable: "JobCard",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SparePartUsed",
                schema: "wrk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SerialNoUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWarrantyClaim = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_SparePartUsed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartUsed_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalSchema: "wrk",
                        principalTable: "JobCard",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarrantyMonths = table.Column<int>(type: "int", nullable: true),
                    IsSerialized = table.Column<bool>(type: "bit", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReorderLevel = table.Column<int>(type: "int", nullable: true),
                    ReorderQuantity = table.Column<int>(type: "int", nullable: true),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATPct = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountLimitPct = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Brand_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "inv",
                        principalTable: "Brand",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inv",
                        principalTable: "ProductCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoiceDetail",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SerialId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_SalesInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceDetail_SalesInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "fin",
                        principalTable: "SalesInvoice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "prc",
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierLedger",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoucherType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherId = table.Column<long>(type: "bigint", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
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
                    TagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierLedger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierLedger_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "prc",
                        principalTable: "Supplier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierLedger_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "prc",
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistory",
                schema: "wf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<long>(type: "bigint", nullable: true),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    StepId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverUserId = table.Column<long>(type: "bigint", nullable: true),
                    Decision = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestDataSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ApprovalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_WorkflowDefinition_WorkflowId",
                        column: x => x.WorkflowId,
                        principalSchema: "wf",
                        principalTable: "WorkflowDefinition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStep",
                schema: "wf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<long>(type: "bigint", nullable: true),
                    StepOrder = table.Column<int>(type: "int", nullable: true),
                    RequiredRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalTimeoutHours = table.Column<int>(type: "int", nullable: true),
                    EscalationRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsParallel = table.Column<bool>(type: "bit", nullable: true),
                    ConditionExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WorkflowStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStep_WorkflowDefinition_WorkflowId",
                        column: x => x.WorkflowId,
                        principalSchema: "wf",
                        principalTable: "WorkflowDefinition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatementLine",
                schema: "bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatementId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MatchedWithPaymentId = table.Column<long>(type: "bigint", nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_StatementLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatementLine_BankStatement_StatementId",
                        column: x => x.StatementId,
                        principalSchema: "bank",
                        principalTable: "BankStatement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
                    PermissionId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "erp_sys",
                        principalTable: "Permission",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "erp_sys",
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "erp_sys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "erp_sys",
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "erp_sys",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalHold",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    HoldReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlacedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    PlacedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleasedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LegalHold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalHold_StoredDocument_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "doc",
                        principalTable: "StoredDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OcrDocumentData",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    ExtractedJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NidNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Confidence = table.Column<float>(type: "real", nullable: true),
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
                    table.PrimaryKey("PK_OcrDocumentData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcrDocumentData_StoredDocument_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "doc",
                        principalTable: "StoredDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JournalLine",
                schema: "acc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalId = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_JournalLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalLine_ChartOfAccount_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "acc",
                        principalTable: "ChartOfAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JournalLine_JournalEntry_JournalId",
                        column: x => x.JournalId,
                        principalSchema: "acc",
                        principalTable: "JournalEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HPAgreement",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgreementNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    AgreementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentCount = table.Column<int>(type: "int", nullable: true),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    GuarantorId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WrittenOffByUserId = table.Column<long>(type: "bigint", nullable: true),
                    WrittenOffAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WrittenOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CollectionOfficerId = table.Column<long>(type: "bigint", nullable: true),
                    SalesPersonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatePaymentPenaltyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalWorkflowId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_HPAgreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HPAgreement_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HPAgreement_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inv",
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SerialNumber",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentHPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseLocationId = table.Column<long>(type: "bigint", nullable: true),
                    LastKnownLatitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastKnownLongitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_SerialNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerialNumber_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inv",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SerialNumber_WarehouseLocation_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalSchema: "inv",
                        principalTable: "WarehouseLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockBalance",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: true),
                    QuantityReserved = table.Column<int>(type: "int", nullable: true),
                    QuantityOrdered = table.Column<int>(type: "int", nullable: true),
                    ReorderLevel = table.Column<int>(type: "int", nullable: true),
                    ReorderQuantity = table.Column<int>(type: "int", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_StockBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockBalance_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inv",
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GRN",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRNNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONo = table.Column<long>(type: "bigint", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChallanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_GRN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRN_PurchaseOrder_PONo",
                        column: x => x.PONo,
                        principalSchema: "prc",
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PODetail",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONo = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReceivedQuantity = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_PODetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PODetail_PurchaseOrder_PONo",
                        column: x => x.PONo,
                        principalSchema: "prc",
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EMISchedule",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    InstallmentNo = table.Column<int>(type: "int", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastReminderSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastReminderType = table.Column<int>(type: "int", nullable: true),
                    IsLegalNoticeSent = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_EMISchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EMISchedule_HPAgreement_HPAgreementId",
                        column: x => x.HPAgreementId,
                        principalSchema: "fin",
                        principalTable: "HPAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecoveryBoard",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    RiskBucket = table.Column<int>(type: "int", nullable: true),
                    TotalOverdue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastFollowupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedCollectorId = table.Column<long>(type: "bigint", nullable: true),
                    PromiseToPayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegalEscalationFlag = table.Column<bool>(type: "bit", nullable: true),
                    LegalCaseId = table.Column<long>(type: "bigint", nullable: true),
                    CollectorNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecoveryScore = table.Column<int>(type: "int", nullable: true),
                    NextFollowupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AiSuggestedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiConfidenceScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_RecoveryBoard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecoveryBoard_HPAgreement_HPAgreementId",
                        column: x => x.HPAgreementId,
                        principalSchema: "fin",
                        principalTable: "HPAgreement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransferDetail",
                schema: "inv",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferId = table.Column<long>(type: "bigint", nullable: true),
                    SerialId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_TransferDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferDetail_SerialNumber_SerialId",
                        column: x => x.SerialId,
                        principalSchema: "inv",
                        principalTable: "SerialNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransferDetail_StockTransfer_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "inv",
                        principalTable: "StockTransfer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GRNDetail",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRNId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityReceived = table.Column<int>(type: "int", nullable: true),
                    UnitCostApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_GRNDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNDetail_GRN_GRNId",
                        column: x => x.GRNId,
                        principalSchema: "prc",
                        principalTable: "GRN",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LandedCost",
                schema: "prc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRNId = table.Column<long>(type: "bigint", nullable: true),
                    CostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LandedCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandedCost_GRN_GRNId",
                        column: x => x.GRNId,
                        principalSchema: "prc",
                        principalTable: "GRN",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HPAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    EMIId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CollectorEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    IsPartial = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
                    SlipImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReconciledWithBank = table.Column<bool>(type: "bit", nullable: true),
                    ReconciledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReconciledByUserId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_EMISchedule_EMIId",
                        column: x => x.EMIId,
                        principalSchema: "fin",
                        principalTable: "EMISchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_HPAgreement_HPAgreementId",
                        column: x => x.HPAgreementId,
                        principalSchema: "fin",
                        principalTable: "HPAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Penalty",
                schema: "fin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMIId = table.Column<long>(type: "bigint", nullable: true),
                    PenaltyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaysOverdue = table.Column<int>(type: "int", nullable: true),
                    PenaltyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsWaived = table.Column<bool>(type: "bit", nullable: true),
                    WaivedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    WaivedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    PaidAgainstPaymentId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Penalty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalty_EMISchedule_EMIId",
                        column: x => x.EMIId,
                        principalSchema: "fin",
                        principalTable: "EMISchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AISentimentLog_CustomerId",
                schema: "crm",
                table: "AISentimentLog",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_WorkflowId",
                schema: "wf",
                table: "ApprovalHistory",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                schema: "hr",
                table: "Attendance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatement_BankAccountId",
                schema: "bank",
                table: "BankStatement",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashDeposit_BankAccountId",
                schema: "bank",
                table: "CashDeposit",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccount_ParentAccountId",
                schema: "acc",
                table: "ChartOfAccount",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ParentCompanyId",
                schema: "erp_sys",
                table: "Company",
                column: "ParentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Phone",
                schema: "crm",
                table: "Customer",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationSchedule_AssetId",
                schema: "acc",
                table: "DepreciationSchedule",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_EMISchedule_DueDate",
                schema: "fin",
                table: "EMISchedule",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_EMISchedule_HPAgreementId",
                schema: "fin",
                table: "EMISchedule",
                column: "HPAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowupTask_CustomerId",
                schema: "crm",
                table: "FollowupTask",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_GRN_PONo",
                schema: "prc",
                table: "GRN",
                column: "PONo");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetail_GRNId",
                schema: "prc",
                table: "GRNDetail",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_Guarantor_CustomerId",
                schema: "fin",
                table: "Guarantor",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_HPAgreement_AgreementNo",
                schema: "fin",
                table: "HPAgreement",
                column: "AgreementNo");

            migrationBuilder.CreateIndex(
                name: "IX_HPAgreement_CustomerId",
                schema: "fin",
                table: "HPAgreement",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_HPAgreement_ProductId",
                schema: "fin",
                table: "HPAgreement",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Incentive_EmployeeId",
                schema: "prl",
                table: "Incentive",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobService_JobCardId",
                schema: "wrk",
                table: "JobService",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_PeriodId",
                schema: "acc",
                table: "JournalEntry",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_VoucherNo",
                schema: "acc",
                table: "JournalEntry",
                column: "VoucherNo");

            migrationBuilder.CreateIndex(
                name: "IX_JournalLine_AccountId",
                schema: "acc",
                table: "JournalLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalLine_JournalId",
                schema: "acc",
                table: "JournalLine",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_LandedCost_GRNId",
                schema: "prc",
                table: "LandedCost",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_EmployeeId",
                schema: "hr",
                table: "Leave",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeId",
                schema: "hr",
                table: "LeaveBalance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalHold_DocumentId",
                schema: "doc",
                table: "LegalHold",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OcrDocumentData_DocumentId",
                schema: "doc",
                table: "OcrDocumentData",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_EMIId",
                schema: "fin",
                table: "Payment",
                column: "EMIId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_HPAgreementId",
                schema: "fin",
                table: "Payment",
                column: "HPAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_EMIId",
                schema: "fin",
                table: "Penalty",
                column: "EMIId");

            migrationBuilder.CreateIndex(
                name: "IX_PODetail_PONo",
                schema: "prc",
                table: "PODetail",
                column: "PONo");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandId",
                schema: "inv",
                table: "Product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                schema: "inv",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ParentCategoryId",
                schema: "inv",
                table: "ProductCategory",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_PONumber",
                schema: "prc",
                table: "PurchaseOrder",
                column: "PONumber");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_VendorId",
                schema: "prc",
                table: "PurchaseOrder",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecoveryBoard_HPAgreementId",
                schema: "fin",
                table: "RecoveryBoard",
                column: "HPAgreementId",
                unique: true,
                filter: "[HPAgreementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ParentCompanyId",
                schema: "erp_sys",
                table: "Role",
                column: "ParentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "erp_sys",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "erp_sys",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalarySlip_EmployeeId",
                schema: "prl",
                table: "SalarySlip",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructure_EmployeeId",
                schema: "prl",
                table: "SalaryStructure",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceDetail_InvoiceId",
                schema: "fin",
                table: "SalesInvoiceDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumber_ProductId",
                schema: "inv",
                table: "SerialNumber",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumber_SerialNo",
                schema: "inv",
                table: "SerialNumber",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumber_WarehouseLocationId",
                schema: "inv",
                table: "SerialNumber",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartUsed_JobCardId",
                schema: "wrk",
                table: "SparePartUsed",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementLine_StatementId",
                schema: "bank",
                table: "StatementLine",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_StockBalance_ProductId",
                schema: "inv",
                table: "StockBalance",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredDocument_CategoryId",
                schema: "doc",
                table: "StoredDocument",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredDocument_CustomerId",
                schema: "doc",
                table: "StoredDocument",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLedger_SupplierId",
                schema: "prc",
                table: "SupplierLedger",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLedger_VendorId",
                schema: "prc",
                table: "SupplierLedger",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetail_SerialId",
                schema: "inv",
                table: "TransferDetail",
                column: "SerialId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetail_TransferId",
                schema: "inv",
                table: "TransferDetail",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "erp_sys",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                schema: "erp_sys",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStep_WorkflowId",
                schema: "wf",
                table: "WorkflowStep",
                column: "WorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AISentimentLog",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "ApprovalHistory",
                schema: "wf");

            migrationBuilder.DropTable(
                name: "Attendance",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "BISnapshot",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "BranchCashClosing",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "Campaign",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CashDeposit",
                schema: "bank");

            migrationBuilder.DropTable(
                name: "DataWarehouseSyncLog",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "DepreciationSchedule",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "FollowupTask",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "ForecastSnapshot",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "GRNDetail",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "Guarantor",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "Incentive",
                schema: "prl");

            migrationBuilder.DropTable(
                name: "InsurancePolicy",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "JobService",
                schema: "wrk");

            migrationBuilder.DropTable(
                name: "JournalLine",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "LandedCost",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "Lead",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Leave",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LeaveBalance",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LegalHold",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "LegalNotice",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "OcrDocumentData",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "Penalty",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "PODetail",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "PromptTemplate",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "RecoveryBoard",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "RepossessionCase",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "SalarySlip",
                schema: "prl");

            migrationBuilder.DropTable(
                name: "SalaryStructure",
                schema: "prl");

            migrationBuilder.DropTable(
                name: "SalesInvoiceDetail",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "SmsQueue",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "SparePartUsed",
                schema: "wrk");

            migrationBuilder.DropTable(
                name: "StatementLine",
                schema: "bank");

            migrationBuilder.DropTable(
                name: "StockAging",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "StockBalance",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "SupplierLedger",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "TechnicianEfficiency",
                schema: "wrk");

            migrationBuilder.DropTable(
                name: "TransferDetail",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "TrialBalance",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "VectorEmbedding",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "VehicleRegistration",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "WarrantyClaim",
                schema: "wrk");

            migrationBuilder.DropTable(
                name: "WhatsAppQueue",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "WorkflowStep",
                schema: "wf");

            migrationBuilder.DropTable(
                name: "FixedAsset",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "ChartOfAccount",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "JournalEntry",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "GRN",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "StoredDocument",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "EMISchedule",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "SalesInvoice",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "JobCard",
                schema: "wrk");

            migrationBuilder.DropTable(
                name: "BankStatement",
                schema: "bank");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "SerialNumber",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "StockTransfer",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "User",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "WorkflowDefinition",
                schema: "wf");

            migrationBuilder.DropTable(
                name: "FiscalPeriod",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "PurchaseOrder",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "DocumentCategory",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "HPAgreement",
                schema: "fin");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "bank");

            migrationBuilder.DropTable(
                name: "WarehouseLocation",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "erp_sys");

            migrationBuilder.DropTable(
                name: "Vendor",
                schema: "prc");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "Brand",
                schema: "inv");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "inv");
        }
    }
}
