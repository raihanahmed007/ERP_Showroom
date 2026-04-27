using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.acc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedAccountingDataAsync(context);
    }

    private static async Task SeedAccountingDataAsync(AppDbContext context)
    {
        if (await context.ChartOfAccounts.AnyAsync(a => a.AccountCode == "1000")) return;

        // 1. Root Heads
        var assets = new ChartOfAccount { AccountCode = "1000", AccountName = "ASSETS", AccountType = AccountTypeEnum.Asset, IsHead = true };
        var liabilities = new ChartOfAccount { AccountCode = "2000", AccountName = "LIABILITIES", AccountType = AccountTypeEnum.Liability, IsHead = true };
        var equity = new ChartOfAccount { AccountCode = "3000", AccountName = "EQUITY", AccountType = AccountTypeEnum.Equity, IsHead = true };
        var income = new ChartOfAccount { AccountCode = "4000", AccountName = "INCOME", AccountType = AccountTypeEnum.Income, IsHead = true };
        var expenses = new ChartOfAccount { AccountCode = "5000", AccountName = "EXPENSES", AccountType = AccountTypeEnum.Expense, IsHead = true };

        context.ChartOfAccounts.AddRange(assets, liabilities, equity, income, expenses);
        await context.SaveChangesAsync();

        // 2. Standard Sub-Heads & Ledgers
        context.ChartOfAccounts.AddRange(
            // Assets
            new ChartOfAccount { AccountCode = "1100", AccountName = "Current Assets", AccountType = AccountTypeEnum.Asset, IsHead = true, ParentAccountId = assets.Id },
            new ChartOfAccount { AccountCode = "1110", AccountName = "Cash in Hand", AccountType = AccountTypeEnum.Asset, IsHead = false, ParentAccountId = assets.Id, OpeningBalance = 50000 },
            new ChartOfAccount { AccountCode = "1120", AccountName = "Bank Accounts", AccountType = AccountTypeEnum.Asset, IsHead = true, ParentAccountId = assets.Id },
            new ChartOfAccount { AccountCode = "1121", AccountName = "Dutch Bangla Bank", AccountType = AccountTypeEnum.Asset, IsHead = false, ParentAccountId = assets.Id, OpeningBalance = 1000000 },
            new ChartOfAccount { AccountCode = "1130", AccountName = "Accounts Receivable", AccountType = AccountTypeEnum.Asset, IsHead = false, ParentAccountId = assets.Id },
            new ChartOfAccount { AccountCode = "1200", AccountName = "Fixed Assets", AccountType = AccountTypeEnum.Asset, IsHead = true, ParentAccountId = assets.Id },
            new ChartOfAccount { AccountCode = "1210", AccountName = "Office Equipment", AccountType = AccountTypeEnum.Asset, IsHead = false, ParentAccountId = assets.Id },
            
            // Liabilities
            new ChartOfAccount { AccountCode = "2100", AccountName = "Current Liabilities", AccountType = AccountTypeEnum.Liability, IsHead = true, ParentAccountId = liabilities.Id },
            new ChartOfAccount { AccountCode = "2110", AccountName = "Accounts Payable", AccountType = AccountTypeEnum.Liability, IsHead = false, ParentAccountId = liabilities.Id },
            new ChartOfAccount { AccountCode = "2120", AccountName = "Accrued Expenses", AccountType = AccountTypeEnum.Liability, IsHead = false, ParentAccountId = liabilities.Id },
            
            // Equity
            new ChartOfAccount { AccountCode = "3100", AccountName = "Share Capital", AccountType = AccountTypeEnum.Equity, IsHead = false, ParentAccountId = equity.Id },
            new ChartOfAccount { AccountCode = "3200", AccountName = "Retained Earnings", AccountType = AccountTypeEnum.Equity, IsHead = false, ParentAccountId = equity.Id },
            
            // Income
            new ChartOfAccount { AccountCode = "4100", AccountName = "Product Sales", AccountType = AccountTypeEnum.Income, IsHead = false, ParentAccountId = income.Id },
            new ChartOfAccount { AccountCode = "4200", AccountName = "Service Income", AccountType = AccountTypeEnum.Income, IsHead = false, ParentAccountId = income.Id },
            
            // Expenses
            new ChartOfAccount { AccountCode = "5100", AccountName = "Cost of Goods Sold", AccountType = AccountTypeEnum.Expense, IsHead = false, ParentAccountId = expenses.Id },
            new ChartOfAccount { AccountCode = "5200", AccountName = "Administrative Expenses", AccountType = AccountTypeEnum.Expense, IsHead = true, ParentAccountId = expenses.Id },
            new ChartOfAccount { AccountCode = "5210", AccountName = "Office Rent", AccountType = AccountTypeEnum.Expense, IsHead = false, ParentAccountId = expenses.Id },
            new ChartOfAccount { AccountCode = "5220", AccountName = "Salaries & Wages", AccountType = AccountTypeEnum.Expense, IsHead = false, ParentAccountId = expenses.Id }
        );

        // 3. Seed an Open Fiscal Period
        if (!await context.FiscalPeriods.AnyAsync())
        {
            context.FiscalPeriods.Add(new FiscalPeriod
            {
                PeriodName = "FY 2024-25",
                StartDate = new DateTime(2024, 7, 1),
                EndDate = new DateTime(2025, 6, 30),
                IsClosed = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }
}
