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

namespace ErpShowroom.Application.acc.Commands;

public record JournalLineDto(long AccountId, decimal Debit, decimal Credit, string Description);

public record CreateJournalEntryCommand(VoucherTypeEnum VoucherType, DateTime JournalDate, string Narration, List<JournalLineDto> Lines) : IRequest<long>;
public class CreateJournalEntryHandler(IApplicationDbContext db) : IRequestHandler<CreateJournalEntryCommand, long> {
    public async Task<long> Handle(CreateJournalEntryCommand request, CancellationToken ct) {
        var entry = new JournalEntry { 
            VoucherType = request.VoucherType, 
            JournalDate = request.JournalDate, 
            Narration = request.Narration, 
            VoucherNo = "JV-" + Guid.NewGuid() 
        };
        db.JournalEntries.Add(entry);
        
        entry.JournalLines = request.Lines.Select(l => new JournalLine {
            AccountId = l.AccountId,
            DebitAmount = l.Debit,
            CreditAmount = l.Credit,
            Description = l.Description
        }).ToList();

        entry.ValidateDebitCredit(); // DDD method trigger
        await db.SaveChangesAsync(ct);
        return entry.Id;
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

public record GetTrialBalanceQuery(long PeriodId) : IRequest<List<TrialBalance>>;
public class GetTrialBalanceHandler(IApplicationDbContext db) : IRequestHandler<GetTrialBalanceQuery, List<TrialBalance>> {
    public async Task<List<TrialBalance>> Handle(GetTrialBalanceQuery request, CancellationToken ct) {
        return await db.TrialBalances.Where(t => t.PeriodId == request.PeriodId).ToListAsync(ct);
    }
}
