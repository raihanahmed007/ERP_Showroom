using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.acc.Entities;
using ErpShowroom.Domain.Common;
using ErpShowroom.Application.acc.DTOs;

namespace ErpShowroom.Application.acc.Commands;

public record CreateJournalEntryCommand(VoucherTypeEnum VoucherType, DateTime JournalDate, string Narration, List<JournalLineDto> Lines) : IRequest<long>;

public class CreateJournalEntryHandler(IApplicationDbContext db) : IRequestHandler<CreateJournalEntryCommand, long> {
    public async Task<long> Handle(CreateJournalEntryCommand request, CancellationToken ct) {
        // Validate Fiscal Period
        var period = await db.FiscalPeriods
            .FirstOrDefaultAsync(p => request.JournalDate >= p.StartDate && request.JournalDate <= p.EndDate && p.IsClosed != true, ct);
        
        if (period == null)
            throw new Exception("Journal date does not fall within an open fiscal period.");

        var prefix = request.VoucherType.ToString();
        var yearMonth = request.JournalDate.ToString("yyMM");
        
        var lastVoucher = await db.JournalEntries
            .Where(e => e.VoucherNo!.StartsWith($"{prefix}/{yearMonth}/"))
            .OrderByDescending(e => e.VoucherNo)
            .FirstOrDefaultAsync(ct);
        
        int nextNum = 1;
        if (lastVoucher != null) {
            var parts = lastVoucher.VoucherNo!.Split('/');
            if (parts.Length == 3 && int.TryParse(parts[2], out int num)) {
                nextNum = num + 1;
            }
        }

        var entry = new JournalEntry { 
            VoucherType = request.VoucherType, 
            JournalDate = request.JournalDate, 
            Narration = request.Narration, 
            PeriodId = period.Id,
            VoucherNo = $"{prefix}/{yearMonth}/{nextNum:D4}",
            Status = VoucherStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
        
        db.JournalEntries.Add(entry);
        
        entry.JournalLines = request.Lines.Select(l => new JournalLine {
            AccountId = l.AccountId,
            DebitAmount = l.DebitAmount,
            CreditAmount = l.CreditAmount,
            Description = l.Description,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        entry.ValidateDebitCredit(); 
        await db.SaveChangesAsync(ct);
        return entry.Id;
    }
}

public record ApproveVoucherCommand(long JournalId, long UserId) : IRequest<bool>;
public class ApproveVoucherHandler(IApplicationDbContext db) : IRequestHandler<ApproveVoucherCommand, bool> {
    public async Task<bool> Handle(ApproveVoucherCommand request, CancellationToken ct) {
        var entry = await db.JournalEntries.Include(x => x.JournalLines).FirstOrDefaultAsync(e => e.Id == request.JournalId, ct);
        if (entry == null || entry.Status != VoucherStatus.Draft) return false;

        entry.Status = VoucherStatus.Posted;
        entry.ApprovedByUserId = request.UserId;
        entry.ApprovedAt = DateTime.UtcNow;
        entry.PostedByUserId = request.UserId;
        entry.PostedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return true;
    }
}

public record CloseFiscalPeriodCommand(long PeriodId) : IRequest<bool>;
public class CloseFiscalPeriodHandler(IApplicationDbContext db) : IRequestHandler<CloseFiscalPeriodCommand, bool> {
    public async Task<bool> Handle(CloseFiscalPeriodCommand request, CancellationToken ct) {
        var period = await db.FiscalPeriods.FirstOrDefaultAsync(p => p.Id == request.PeriodId, ct);
        if (period != null) { 
            period.IsClosed = true; 
            period.ClosedAt = DateTime.UtcNow; 
            await db.SaveChangesAsync(ct); 
            return true; 
        }
        return false;
    }
}

public record RecordBranchCashClosingCommand(DateTime Date, decimal ClosingCash, string Remarks) : IRequest<long>;
public class RecordBranchCashClosingHandler(IApplicationDbContext db) : IRequestHandler<RecordBranchCashClosingCommand, long> {
    public async Task<long> Handle(RecordBranchCashClosingCommand request, CancellationToken ct) {
        var closing = new BranchCashClosing {
            ClosingDate = request.Date,
            ClosingCash = request.ClosingCash,
            Remarks = request.Remarks,
            CreatedAt = DateTime.UtcNow
        };
        db.BranchCashClosings.Add(closing);
        await db.SaveChangesAsync(ct);
        return closing.Id;
    }
}

public record CreateAccountCommand(string AccountCode, string AccountName, AccountTypeEnum? AccountType, long? ParentAccountId, bool IsHead, decimal? OpeningBalance, string? CurrencyCode, string? Notes) : IRequest<long>;
public class CreateAccountHandler(IApplicationDbContext db) : IRequestHandler<CreateAccountCommand, long> {
    public async Task<long> Handle(CreateAccountCommand request, CancellationToken ct) {
        var account = new ChartOfAccount {
            AccountCode = request.AccountCode,
            AccountName = request.AccountName,
            AccountType = request.AccountType,
            ParentAccountId = request.ParentAccountId,
            IsHead = request.IsHead,
            OpeningBalance = request.OpeningBalance,
            CurrencyCode = request.CurrencyCode ?? "BDT",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };
        db.ChartOfAccounts.Add(account);
        await db.SaveChangesAsync(ct);
        return account.Id;
    }
}

public record UpdateAccountCommand(long Id, string AccountCode, string AccountName, AccountTypeEnum? AccountType, long? ParentAccountId, bool IsHead, decimal? OpeningBalance, string? CurrencyCode, string? Notes) : IRequest<bool>;
public class UpdateAccountHandler(IApplicationDbContext db) : IRequestHandler<UpdateAccountCommand, bool> {
    public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken ct) {
        var account = await db.ChartOfAccounts.FirstOrDefaultAsync(a => a.Id == request.Id, ct);
        if (account == null) return false;

        // Protect system root heads
        string[] protectedCodes = { "1000", "2000", "3000", "4000", "5000" };
        if (protectedCodes.Contains(account.AccountCode)) return false;

        account.AccountCode = request.AccountCode;
        account.AccountName = request.AccountName;
        account.AccountType = request.AccountType;
        account.ParentAccountId = request.ParentAccountId;
        account.IsHead = request.IsHead;
        account.OpeningBalance = request.OpeningBalance;
        account.CurrencyCode = request.CurrencyCode ?? "BDT";
        account.Notes = request.Notes;
        account.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return true;
    }
}

public record DeleteAccountCommand(long Id) : IRequest<bool>;
public class DeleteAccountHandler(IApplicationDbContext db) : IRequestHandler<DeleteAccountCommand, bool> {
    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken ct) {
        var account = await db.ChartOfAccounts.FirstOrDefaultAsync(a => a.Id == request.Id, ct);
        if (account == null) return false;
        
        // Protect system accounts
        string[] protectedCodes = { "1000", "1200", "1300", "2000", "3000", "4000", "4001", "4100", "5000", "5100" };
        if (protectedCodes.Contains(account.AccountCode)) return false;
        
        // Check if any journal lines exist
        var hasTransactions = await db.JournalLines.AnyAsync(l => l.AccountId == request.Id, ct);
        if (hasTransactions) return false;

        db.ChartOfAccounts.Remove(account);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
