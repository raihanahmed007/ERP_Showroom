using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.bank.Entities;

namespace ErpShowroom.Application.bank.Commands;

public record ReconciliationMatch(long StatementLineId, long PaymentId);

public record ReconcileBankStatementCommand(long BankStatementId, List<ReconciliationMatch> Matches) : IRequest<bool>;
public class ReconcileBankStatementHandler(IApplicationDbContext db) : IRequestHandler<ReconcileBankStatementCommand, bool> {
    public async Task<bool> Handle(ReconcileBankStatementCommand request, CancellationToken ct) {
        var statement = await db.BankStatements.Include(s => s.Lines).FirstOrDefaultAsync(s => s.Id == request.BankStatementId, ct);
        if (statement == null) return false;

        foreach(var match in request.Matches) {
            var line = statement.Lines?.FirstOrDefault(l => l.Id == match.StatementLineId);
            if (line != null) {
                line.MatchedWithPaymentId = match.PaymentId;
            }
        }
        
        statement.IsReconciled = true;
        statement.ReconciledAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(ct);
        return true;
    }
}
