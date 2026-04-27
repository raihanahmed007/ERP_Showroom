using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.acc.Entities;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.hr.Entities;
using ErpShowroom.Domain.prl.Entities;
using ErpShowroom.Domain.doc.Entities;
using ErpShowroom.Domain.bank.Entities;
using ErpShowroom.Domain.wf.Entities;

namespace ErpShowroom.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // sys
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<AppModule> Modules => Set<AppModule>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<AppPage> Pages => Set<AppPage>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SmsQueue> SmsQueues => Set<SmsQueue>();
    public DbSet<WhatsAppQueue> WhatsAppQueues => Set<WhatsAppQueue>();
    public DbSet<BISnapshot> BISnapshots => Set<BISnapshot>();
    public DbSet<ForecastSnapshot> ForecastSnapshots => Set<ForecastSnapshot>();
    public DbSet<DataWarehouseSyncLog> DataWarehouseSyncLogs => Set<DataWarehouseSyncLog>();
    public DbSet<VectorEmbedding> VectorEmbeddings => Set<VectorEmbedding>();
    public DbSet<PromptTemplate> PromptTemplates => Set<PromptTemplate>();
    public DbSet<CompanyModuleAccess> CompanyModuleAccesses => Set<CompanyModuleAccess>();
    public DbSet<PagePermission> PagePermissions => Set<PagePermission>();
    public DbSet<ApprovalSetup> ApprovalSetups => Set<ApprovalSetup>();
    public DbSet<UserPageAccess> UserPageAccesses => Set<UserPageAccess>();
    public DbSet<ReportType> ReportTypes => Set<ReportType>();
    public DbSet<ReportName> ReportNames => Set<ReportName>();
    public DbSet<UserDashboardAccess> UserDashboardAccesses => Set<UserDashboardAccess>();
    public DbSet<DatabaseBackupLog> DatabaseBackupLogs => Set<DatabaseBackupLog>();
    public DbSet<UserLoginLog> UserLoginLogs => Set<UserLoginLog>();
    public DbSet<SoftDeleteRegistry> SoftDeleteRegistries => Set<SoftDeleteRegistry>();
    public DbSet<UserReportPermission> UserReportPermissions => Set<UserReportPermission>();
    
    // acc
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine> JournalLines => Set<JournalLine>();
    public DbSet<TrialBalance> TrialBalances => Set<TrialBalance>();
    public DbSet<BranchCashClosing> BranchCashClosings => Set<BranchCashClosing>();
    public DbSet<FixedAsset> FixedAssets => Set<FixedAsset>();
    public DbSet<DepreciationSchedule> DepreciationSchedules => Set<DepreciationSchedule>();

    // fin
    public DbSet<HPAgreement> HPAgreements => Set<HPAgreement>();
    public DbSet<EMISchedule> EMISchedules => Set<EMISchedule>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Penalty> Penalties => Set<Penalty>();
    public DbSet<RecoveryBoard> RecoveryBoards => Set<RecoveryBoard>();
    public DbSet<Guarantor> Guarantors => Set<Guarantor>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceDetail> SalesInvoiceDetails => Set<SalesInvoiceDetail>();
    public DbSet<LegalNotice> LegalNotices => Set<LegalNotice>();
    public DbSet<RepossessionCase> RepossessionCases => Set<RepossessionCase>();
    public DbSet<VehicleRegistration> VehicleRegistrations => Set<VehicleRegistration>();
    public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();

    // inv
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<SerialNumber> SerialNumbers => Set<SerialNumber>();
    public DbSet<StockBalance> StockBalances => Set<StockBalance>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<TransferDetail> TransferDetails => Set<TransferDetail>();
    public DbSet<StockAging> StockAgings => Set<StockAging>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();

    // prc
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PODetail> PODetails => Set<PODetail>();
    public DbSet<GRN> GRNs => Set<GRN>();
    public DbSet<GRNDetail> GRNDetails => Set<GRNDetail>();
    public DbSet<LandedCost> LandedCosts => Set<LandedCost>();
    public DbSet<SupplierLedger> SupplierLedgers => Set<SupplierLedger>();

    // crm
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<AISentimentLog> AISentimentLogs => Set<AISentimentLog>();
    public DbSet<FollowupTask> FollowupTasks => Set<FollowupTask>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();

    // wrk
    public DbSet<JobCard> JobCards => Set<JobCard>();
    public DbSet<JobService> JobServices => Set<JobService>();
    public DbSet<SparePartUsed> SparePartsUseds => Set<SparePartUsed>();
    public DbSet<TechnicianEfficiency> TechnicianEfficiencies => Set<TechnicianEfficiency>();
    public DbSet<WarrantyClaim> WarrantyClaims => Set<WarrantyClaim>();

    // hr
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Leave> Leaves => Set<Leave>();
    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

    // prl
    public DbSet<SalaryStructure> SalaryStructures => Set<SalaryStructure>();
    public DbSet<SalarySlip> SalarySlips => Set<SalarySlip>();
    public DbSet<Incentive> Incentives => Set<Incentive>();

    // doc
    public DbSet<DocumentCategory> DocumentCategories => Set<DocumentCategory>();
    public DbSet<StoredDocument> StoredDocuments => Set<StoredDocument>();
    public DbSet<LegalHold> LegalHolds => Set<LegalHold>();
    public DbSet<OcrDocumentData> OcrDocumentDatas => Set<OcrDocumentData>();

    // bank
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<BankStatement> BankStatements => Set<BankStatement>();
    public DbSet<StatementLine> StatementLines => Set<StatementLine>();
    public DbSet<CashDeposit> CashDeposits => Set<CashDeposit>();

    // wf
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
    public DbSet<ApprovalHistory> ApprovalHistories => Set<ApprovalHistory>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1. Configure Table Names, Schemas, and Soft Delete filters dynamically
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;

            if (type.Namespace != null && type.Namespace.Contains("ErpShowroom.Domain."))
            {
                var nsParts = type.Namespace.Split('.');
                if (nsParts.Length >= 4)
                {
                    var schema = nsParts[2];
                    if (!string.Equals(schema, "Common", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(schema, "sys", StringComparison.OrdinalIgnoreCase))
                        {
                            schema = "erp_sys";
                        }

                        entityType.SetSchema(schema);
                    }

                    entityType.SetTableName(type.Name);
                }
            }

            if (typeof(BaseEntity).IsAssignableFrom(type))
            {
                var parameter = Expression.Parameter(type, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                    Expression.Constant(false, typeof(bool?))
                );

                var filterLambda = Expression.Lambda(body, parameter);
                entityType.SetQueryFilter(filterLambda);
            }
        }

        // 2. Add Explicit Indexes
        builder.Entity<HPAgreement>().HasIndex(e => e.AgreementNo).HasDatabaseName("IX_HPAgreement_AgreementNo");
        builder.Entity<EMISchedule>().HasIndex(e => e.DueDate).HasDatabaseName("IX_EMISchedule_DueDate");
        builder.Entity<SerialNumber>().HasIndex(e => e.SerialNo).HasDatabaseName("IX_SerialNumber_SerialNo");
        builder.Entity<Customer>().HasIndex(e => e.Phone).HasDatabaseName("IX_Customer_Phone");
        builder.Entity<JournalEntry>().HasIndex(e => e.VoucherNo).HasDatabaseName("IX_JournalEntry_VoucherNo");
        builder.Entity<PurchaseOrder>().HasIndex(e => e.PONumber).HasDatabaseName("IX_PurchaseOrder_PONumber");

        // 3. Configure Relationships using Fluent API (to enforce behaviors like Restrict deletes for critical links)
        builder.Entity<HPAgreement>()
            .HasOne(h => h.Customer)
            .WithMany(c => c.HPAgreements)
            .HasForeignKey(h => h.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<EMISchedule>()
            .HasOne(e => e.Agreement)
            .WithMany(h => h.EMISchedules)
            .HasForeignKey(e => e.HPAgreementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Payment>()
            .HasOne(p => p.EMI)
            .WithMany(e => e.Payments)
            .HasForeignKey(p => p.EMIId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>()
            .HasOne(p => p.Agreement)
            .WithMany(h => h.Payments)
            .HasForeignKey(p => p.HPAgreementId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<JournalLine>()
            .HasOne(jl => jl.Journal)
            .WithMany(j => j.JournalLines)
            .HasForeignKey(jl => jl.JournalId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Entity<SerialNumber>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Serials)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
