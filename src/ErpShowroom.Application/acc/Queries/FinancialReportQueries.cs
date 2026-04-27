using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.acc.DTOs;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.acc.Queries;

// --- Chart of Accounts Queries ---
public record GetChartOfAccountsQuery : IRequest<List<ChartOfAccountDto>>;
public class GetChartOfAccountsHandler(IApplicationDbContext db) : IRequestHandler<GetChartOfAccountsQuery, List<ChartOfAccountDto>> {
    public async Task<List<ChartOfAccountDto>> Handle(GetChartOfAccountsQuery request, CancellationToken ct) {
        return await db.ChartOfAccounts
            .OrderBy(a => a.AccountCode)
            .Select(a => new ChartOfAccountDto(a.Id, a.AccountCode!, a.AccountName!, a.AccountType, a.ParentAccountId, a.IsHead ?? false, a.OpeningBalance))
            .ToListAsync(ct);
    }
}

// --- Ledger Queries ---
public record GetGeneralLedgerQuery(long AccountId, DateTime FromDate, DateTime ToDate) : IRequest<List<LedgerEntryDto>>;
public class GetGeneralLedgerHandler(IApplicationDbContext db) : IRequestHandler<GetGeneralLedgerQuery, List<LedgerEntryDto>> {
    public async Task<List<LedgerEntryDto>> Handle(GetGeneralLedgerQuery request, CancellationToken ct) {
        // Calculate Opening Balance (Sum of all posted entries before FromDate)
        var openingDebit = await db.JournalLines
            .Where(l => l.AccountId == request.AccountId && l.Journal!.JournalDate < request.FromDate && l.Journal.Status == VoucherStatus.Posted)
            .SumAsync(l => l.DebitAmount ?? 0, ct);
            
        var openingCredit = await db.JournalLines
            .Where(l => l.AccountId == request.AccountId && l.Journal!.JournalDate < request.FromDate && l.Journal.Status == VoucherStatus.Posted)
            .SumAsync(l => l.CreditAmount ?? 0, ct);
            
        // Include the Account's Opening Balance defined in COA
        var account = await db.ChartOfAccounts.FirstOrDefaultAsync(a => a.Id == request.AccountId, ct);
        decimal initialBalance = 0;
        if (account != null) {
            if (account.AccountType == AccountTypeEnum.Asset || account.AccountType == AccountTypeEnum.Expense)
                initialBalance = account.OpeningBalance ?? 0;
            else
                initialBalance = (account.OpeningBalance ?? 0) * -1;
        }

        decimal runningBalance = initialBalance + (openingDebit - openingCredit);
        
        var list = new List<LedgerEntryDto>();
        list.Add(new LedgerEntryDto(request.FromDate.AddDays(-1), "OPENING", "Opening Balance", 0, 0, runningBalance, ""));

        // Fetch Transactions
        var lines = await db.JournalLines
            .Where(l => l.AccountId == request.AccountId && l.Journal!.JournalDate >= request.FromDate && l.Journal.JournalDate <= request.ToDate && l.Journal.Status == VoucherStatus.Posted)
            .OrderBy(l => l.Journal!.JournalDate)
            .ThenBy(l => l.Journal!.Id)
            .Select(l => new {
                Date = l.Journal!.JournalDate ?? DateTime.MinValue,
                VoucherNo = l.Journal.VoucherNo!,
                Narration = l.Journal.Narration!,
                Debit = l.DebitAmount ?? 0,
                Credit = l.CreditAmount ?? 0,
                Contra = db.JournalLines
                    .Where(cl => cl.JournalId == l.JournalId && cl.AccountId != l.AccountId)
                    .OrderByDescending(cl => (cl.DebitAmount ?? 0) + (cl.CreditAmount ?? 0))
                    .Select(cl => cl.Account!.AccountName)
                    .FirstOrDefault() ?? ""
            })
            .ToListAsync(ct);

        foreach (var l in lines) {
            runningBalance += (l.Debit - l.Credit);
            list.Add(new LedgerEntryDto(l.Date, l.VoucherNo, l.Narration, l.Debit, l.Credit, runningBalance, l.Contra));
        }

        return list;
    }
}

// --- Financial Statement Queries ---

// Trial Balance
public record GetTrialBalanceQuery(DateTime AsOfDate) : IRequest<List<TrialBalanceDto>>;
public class GetTrialBalanceHandler(IApplicationDbContext db) : IRequestHandler<GetTrialBalanceQuery, List<TrialBalanceDto>> {
    public async Task<List<TrialBalanceDto>> Handle(GetTrialBalanceQuery request, CancellationToken ct) {
        // We calculate balances by aggregating all posted journal lines
        var results = await db.JournalLines
            .Where(l => l.Journal!.JournalDate <= request.AsOfDate && l.Journal.Status == VoucherStatus.Posted)
            .GroupBy(l => new { l.Account!.AccountCode, l.Account.AccountName, l.Account.AccountType, l.Account.OpeningBalance })
            .Select(g => new {
                g.Key.AccountCode,
                g.Key.AccountName,
                g.Key.AccountType,
                g.Key.OpeningBalance,
                Debit = g.Sum(x => x.DebitAmount ?? 0),
                Credit = g.Sum(x => x.CreditAmount ?? 0)
            })
            .ToListAsync(ct);
            
        var final = results.Select(r => {
            decimal d = r.Debit;
            decimal c = r.Credit;
            // Apply opening balance from COA
            if (r.AccountType == AccountTypeEnum.Asset || r.AccountType == AccountTypeEnum.Expense)
                d += r.OpeningBalance ?? 0;
            else
                c += r.OpeningBalance ?? 0;
                
            return new TrialBalanceDto(r.AccountCode!, r.AccountName!, d, c);
        }).OrderBy(x => x.AccountCode).ToList();
            
        return final;
    }
}

// Profit & Loss
public record GetProfitAndLossQuery(DateTime FromDate, DateTime ToDate) : IRequest<List<FinancialReportDto>>;
public class GetProfitAndLossHandler(IApplicationDbContext db) : IRequestHandler<GetProfitAndLossQuery, List<FinancialReportDto>> {
    public async Task<List<FinancialReportDto>> Handle(GetProfitAndLossQuery request, CancellationToken ct) {
        var report = new List<FinancialReportDto>();
        
        var incomeAccounts = await GetAccountBalances(AccountTypeEnum.Income, request.FromDate, request.ToDate, ct);
        var expenseAccounts = await GetAccountBalances(AccountTypeEnum.Expense, request.FromDate, request.ToDate, ct);
        
        report.Add(new FinancialReportDto("INC", "REVENUE", 0, true, 0));
        foreach(var a in incomeAccounts) report.Add(new FinancialReportDto(a.Code, a.Name, a.Balance * -1, false, 1));
        decimal totalIncome = incomeAccounts.Sum(a => a.Balance * -1);
        report.Add(new FinancialReportDto("", "TOTAL REVENUE", totalIncome, true, 0));
        
        report.Add(new FinancialReportDto("EXP", "EXPENSES", 0, true, 0));
        foreach(var a in expenseAccounts) report.Add(new FinancialReportDto(a.Code, a.Name, a.Balance, false, 1));
        decimal totalExpense = expenseAccounts.Sum(a => a.Balance);
        report.Add(new FinancialReportDto("", "TOTAL EXPENSES", totalExpense, true, 0));
        
        report.Add(new FinancialReportDto("NET", "NET PROFIT / LOSS", totalIncome - totalExpense, true, 0));
        
        return report;
    }

    private async Task<List<AccountBalance>> GetAccountBalances(AccountTypeEnum type, DateTime start, DateTime end, CancellationToken ct) {
        return await db.JournalLines
            .Where(l => l.Account!.AccountType == type && l.Journal!.JournalDate >= start && l.Journal.JournalDate <= end && l.Journal.Status == VoucherStatus.Posted)
            .GroupBy(l => new { l.Account!.AccountCode, l.Account.AccountName })
            .Select(g => new AccountBalance {
                Code = g.Key.AccountCode!,
                Name = g.Key.AccountName!,
                Balance = g.Sum(x => (x.DebitAmount ?? 0) - (x.CreditAmount ?? 0))
            })
            .ToListAsync(ct);
    }
}

// Balance Sheet
public record GetBalanceSheetQuery(DateTime AsOfDate) : IRequest<List<FinancialReportDto>>;
public class GetBalanceSheetHandler(IApplicationDbContext db) : IRequestHandler<GetBalanceSheetQuery, List<FinancialReportDto>> {
    public async Task<List<FinancialReportDto>> Handle(GetBalanceSheetQuery request, CancellationToken ct) {
        var report = new List<FinancialReportDto>();

        var assets = await GetAccountBalances(AccountTypeEnum.Asset, request.AsOfDate, ct);
        var liabilities = await GetAccountBalances(AccountTypeEnum.Liability, request.AsOfDate, ct);
        var equity = await GetAccountBalances(AccountTypeEnum.Equity, request.AsOfDate, ct);
        
        var income = await GetTypeTotal(AccountTypeEnum.Income, request.AsOfDate, ct);
        var expense = await GetTypeTotal(AccountTypeEnum.Expense, request.AsOfDate, ct);
        decimal netProfit = (income * -1) - expense;

        report.Add(new FinancialReportDto("AST", "ASSETS", 0, true, 0));
        foreach (var a in assets) report.Add(new FinancialReportDto(a.Code, a.Name, a.Balance, false, 1));
        decimal totalAssets = assets.Sum(a => a.Balance);
        report.Add(new FinancialReportDto("", "TOTAL ASSETS", totalAssets, true, 0));

        report.Add(new FinancialReportDto("LIA", "LIABILITIES", 0, true, 0));
        foreach (var l in liabilities) report.Add(new FinancialReportDto(l.Code, l.Name, l.Balance * -1, false, 1));
        decimal totalLiabilities = liabilities.Sum(l => l.Balance * -1);
        report.Add(new FinancialReportDto("", "TOTAL LIABILITIES", totalLiabilities, true, 0));

        report.Add(new FinancialReportDto("EQU", "EQUITY", 0, true, 0));
        foreach (var e in equity) report.Add(new FinancialReportDto(e.Code, e.Name, e.Balance * -1, false, 1));
        report.Add(new FinancialReportDto("RE", "Retained Earnings (Net Profit)", netProfit, false, 1));
        decimal totalEquity = equity.Sum(e => e.Balance * -1) + netProfit;
        report.Add(new FinancialReportDto("", "TOTAL EQUITY", totalEquity, true, 0));
        
        report.Add(new FinancialReportDto("", "TOTAL LIABILITIES & EQUITY", totalLiabilities + totalEquity, true, 0));

        return report;
    }

    private async Task<List<AccountBalance>> GetAccountBalances(AccountTypeEnum type, DateTime asOf, CancellationToken ct) {
        return await db.JournalLines
            .Where(l => l.Account!.AccountType == type && l.Journal!.JournalDate <= asOf && l.Journal.Status == VoucherStatus.Posted)
            .GroupBy(l => new { l.Account!.AccountCode, l.Account.AccountName, l.Account.OpeningBalance })
            .Select(g => new AccountBalance {
                Code = g.Key.AccountCode!,
                Name = g.Key.AccountName!,
                Balance = (g.Key.OpeningBalance ?? 0) + g.Sum(x => (x.DebitAmount ?? 0) - (x.CreditAmount ?? 0))
            })
            .ToListAsync(ct);
    }
    
    private async Task<decimal> GetTypeTotal(AccountTypeEnum type, DateTime asOf, CancellationToken ct) {
        var baseTotal = await db.JournalLines
            .Where(l => l.Account!.AccountType == type && l.Journal!.JournalDate <= asOf && l.Journal.Status == VoucherStatus.Posted)
            .SumAsync(l => (l.DebitAmount ?? 0) - (l.CreditAmount ?? 0), ct);
            
        var openingTotal = await db.ChartOfAccounts
            .Where(a => a.AccountType == type && !(a.IsHead ?? false))
            .SumAsync(a => a.OpeningBalance ?? 0, ct);
            
        return baseTotal + openingTotal;
    }
}

internal class AccountBalance {
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Balance { get; set; }
}
