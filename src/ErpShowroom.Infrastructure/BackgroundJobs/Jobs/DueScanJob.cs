using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Infrastructure.Persistence;
using ErpShowroom.Application.fin.Workflows;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Infrastructure.BackgroundJobs.Jobs;

public class DueScanJob
{
    private readonly AppDbContext _context;
    private readonly WorkflowOrchestrator _orchestrator;

    public DueScanJob(AppDbContext context, WorkflowOrchestrator orchestrator)
    {
        _context = context;
        _orchestrator = orchestrator;
    }

    public async Task ExecuteAsync()
    {
        var overdueEmis = await _context.EMISchedules
            .Include(e => e.Agreement)
            .Where(e => e.DueDate < DateTime.UtcNow && e.Status != EMIPaymentStatus.Paid)
            .OrderBy(e => e.DueDate)
            .ToListAsync();

        foreach (var group in overdueEmis.GroupBy(e => e.HPAgreementId))
        {
            var firstOverdue = group.First();
            int daysOverdue = (DateTime.UtcNow - firstOverdue.DueDate!.Value).Days;

            if (firstOverdue.HPAgreementId.HasValue)
            {
                await _orchestrator.HandleOverdueEscalationAsync(firstOverdue.HPAgreementId.Value, daysOverdue);
            }
        }
    }
}
