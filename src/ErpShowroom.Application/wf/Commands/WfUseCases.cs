using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.wf.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.wf.Commands;

public record SubmitForApprovalCommand(long WorkflowId, long EntityId) : IRequest<long>;
public class SubmitForApprovalHandler(IApplicationDbContext db) : IRequestHandler<SubmitForApprovalCommand, long> {
    public async Task<long> Handle(SubmitForApprovalCommand request, CancellationToken ct) {
        var history = new ApprovalHistory {
            WorkflowId = request.WorkflowId,
            EntityId = request.EntityId,
            Decision = ApprovalDecision.Pending,
            ActionAt = DateTime.UtcNow
        };
        db.ApprovalHistories.Add(history);
        await db.SaveChangesAsync(ct);
        return history.Id;
    }
}

public record ApproveStepCommand(long ApprovalHistoryId, ApprovalDecision Decision, string Comments) : IRequest<bool>;
public class ApproveStepHandler(IApplicationDbContext db) : IRequestHandler<ApproveStepCommand, bool> {
    public async Task<bool> Handle(ApproveStepCommand request, CancellationToken ct) {
        var history = await db.ApprovalHistories.FirstOrDefaultAsync(h => h.Id == request.ApprovalHistoryId, ct);
        if (history == null) return false;
        
        history.Decision = request.Decision;
        history.Comments = request.Comments;
        history.ActionAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(ct);
        return true;
    }
}
