using System;
using System.Collections.Generic;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.acc.DTOs;

public record ChartOfAccountDto(long Id, string AccountCode, string AccountName, AccountTypeEnum? AccountType, long? ParentAccountId, bool IsHead, decimal? OpeningBalance);

public record JournalLineDto(long AccountId, decimal DebitAmount, decimal CreditAmount, string? Description, string? AccountName = null);

public record JournalEntryDto(long Id, string VoucherNo, VoucherTypeEnum VoucherType, DateTime JournalDate, string Narration, decimal TotalDebit, decimal TotalCredit, VoucherStatus Status, List<JournalLineDto> JournalLines);


public record LedgerEntryDto(DateTime Date, string VoucherNo, string Narration, decimal Debit, decimal Credit, decimal Balance, string ContraAccount = "");

public record TrialBalanceDto(string AccountCode, string AccountName, decimal Debit, decimal Credit);

public record FinancialReportDto(string AccountCode, string AccountName, decimal Amount, bool IsHeader = false, int Level = 0);

public record FiscalPeriodDto(long Id, string PeriodName, DateTime StartDate, DateTime EndDate, bool IsClosed);
