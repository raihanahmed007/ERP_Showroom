using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErpShowroom.Application.acc.DTOs;
using ErpShowroom.Domain.acc.Entities;

using ErpShowroom.Application.acc.Commands;

namespace ErpShowroom.Client.Services.acc;

public interface IAccountingService
{
    Task<List<ChartOfAccountDto>> GetAccountsAsync();
    Task<bool> CreateAccountAsync(CreateAccountCommand command);
    Task<bool> UpdateAccountAsync(UpdateAccountCommand command);
    Task<bool> DeleteAccountAsync(long id);
    Task<long> CreateJournalEntryAsync(CreateJournalEntryCommand command);
    Task<List<TrialBalanceDto>> GetTrialBalanceAsync(DateTime date);
    Task<List<LedgerEntryDto>> GetLedgerAsync(long accountId, DateTime startDate, DateTime endDate);
    Task<List<FinancialReportDto>> GetProfitLossAsync(DateTime startDate, DateTime endDate);
    Task<List<FinancialReportDto>> GetBalanceSheetAsync(DateTime date);
}

public class AccountingService : IAccountingService
{
    private readonly IApiClient _apiClient;

    public AccountingService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<ChartOfAccountDto>> GetAccountsAsync()
    {
        return await _apiClient.GetAsync<List<ChartOfAccountDto>>("api/accounting/accounts") ?? new();
    }

    public async Task<bool> CreateAccountAsync(CreateAccountCommand command)
    {
        var id = await _apiClient.PostAsync<long>("api/accounting/accounts", command);
        return id > 0;
    }

    public async Task<bool> UpdateAccountAsync(UpdateAccountCommand command)
    {
        return await _apiClient.PutAsync($"api/accounting/accounts/{command.Id}", command);
    }

    public async Task<bool> DeleteAccountAsync(long id)
    {
        return await _apiClient.DeleteAsync($"api/accounting/accounts/{id}");
    }

    public async Task<long> CreateJournalEntryAsync(CreateJournalEntryCommand command)
    {
        return await _apiClient.PostAsync<long>("api/accounting/journal-entries", command);
    }

    public async Task<List<TrialBalanceDto>> GetTrialBalanceAsync(DateTime date)
    {
        return await _apiClient.GetAsync<List<TrialBalanceDto>>($"api/accounting/trial-balance?date={date:yyyy-MM-dd}") ?? new();
    }

    public async Task<List<LedgerEntryDto>> GetLedgerAsync(long accountId, DateTime startDate, DateTime endDate)
    {
        return await _apiClient.GetAsync<List<LedgerEntryDto>>($"api/accounting/ledger?accountId={accountId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}") ?? new();
    }

    public async Task<List<FinancialReportDto>> GetProfitLossAsync(DateTime startDate, DateTime endDate)
    {
        return await _apiClient.GetAsync<List<FinancialReportDto>>($"api/accounting/profit-loss?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}") ?? new();
    }

    public async Task<List<FinancialReportDto>> GetBalanceSheetAsync(DateTime date)
    {
        return await _apiClient.GetAsync<List<FinancialReportDto>>($"api/accounting/balance-sheet?date={date:yyyy-MM-dd}") ?? new();
    }
}
