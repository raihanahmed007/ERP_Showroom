using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

namespace ErpShowroom.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // sys
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<Branch> Branches { get; }
    
    // acc
    DbSet<ChartOfAccount> ChartOfAccounts { get; }
    DbSet<FiscalPeriod> FiscalPeriods { get; }
    DbSet<JournalEntry> JournalEntries { get; }
    DbSet<JournalLine> JournalLines { get; }
    DbSet<TrialBalance> TrialBalances { get; }

    // fin
    DbSet<HPAgreement> HPAgreements { get; }
    DbSet<EMISchedule> EMISchedules { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Penalty> Penalties { get; }
    DbSet<RecoveryBoard> RecoveryBoards { get; }

    // inv
    DbSet<Product> Products { get; }
    DbSet<SerialNumber> SerialNumbers { get; }
    DbSet<StockBalance> StockBalances { get; }
    DbSet<StockTransfer> StockTransfers { get; }

    // prc
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<PODetail> PODetails { get; }
    DbSet<GRN> GRNs { get; }

    // crm
    DbSet<Customer> Customers { get; }
    DbSet<Lead> Leads { get; }
    DbSet<AISentimentLog> AISentimentLogs { get; }

    // wrk
    DbSet<JobCard> JobCards { get; }
    DbSet<JobService> JobServices { get; }

    // hr
    DbSet<Employee> Employees { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<Leave> Leaves { get; }

    // prl
    DbSet<SalarySlip> SalarySlips { get; }

    // doc
    DbSet<DocumentCategory> DocumentCategories { get; }
    DbSet<StoredDocument> StoredDocuments { get; }

    // bank
    DbSet<BankAccount> BankAccounts { get; }
    DbSet<BankStatement> BankStatements { get; }

    // wf
    DbSet<WorkflowDefinition> WorkflowDefinitions { get; }
    DbSet<ApprovalHistory> ApprovalHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
