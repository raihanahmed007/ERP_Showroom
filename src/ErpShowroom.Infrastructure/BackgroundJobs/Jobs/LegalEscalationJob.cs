using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Infrastructure.BackgroundJobs.Jobs
{
    // We assume LegalNotice entity exists in Domain somewhere (e.g. fin or doc)
// If it implies dynamic setting we can use Set<LegalNotice>() or assume it's exposed on DbContext.
// Let's assume it exists as ErpShowroom.Domain.fin.Entities.LegalNotice
// For safety we add a local class if user said "Assume entity classes and enums exist".
// Wait, prompt says: "Assume entity classes and enums exist as previously defined".
// So LegalNotice must exist or the user will add it. We'll use a dynamic DBContext approach if it's not exposed as a DbSet, but let's assume it's a DbSet.

public class LegalEscalationJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<LegalEscalationJob> _logger;

    public LegalEscalationJob(IApplicationDbContext context, ILogger<LegalEscalationJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ExecuteAsync()
    {
        try
        {
            var agreementsToEscalate = await _context.RecoveryBoards
                .Include(rb => rb.Agreement)
                .Where(rb => rb.RiskBucket == RiskBucketEnum.Days90Plus && rb.LegalEscalationFlag == false)
                .ToListAsync();

            var now = DateTime.UtcNow;
            int count = 0;

            foreach (var board in agreementsToEscalate)
            {
                if (board.Agreement == null) continue;

                var notice = new ErpShowroom.Domain.fin.Entities.LegalNotice
                {
                    NoticeNo = $"LEG-{board.Agreement.AgreementNo}-{now:yyyyMMdd}",
                    NoticeDate = now,
                    ResponseDeadline = now.AddDays(15),
                    NoticeType = "Final Demand",
                    SentVia = "RegisteredPost",
                    DocumentPath = null,
                    HPAgreementId = board.HPAgreementId,
                    IsActive = true
                };

                _context.LegalNotices.Add(notice);

                board.LegalEscalationFlag = true;

                // Queue internal Notification
                var notification = new ErpShowroom.Domain.sys.Entities.Notification
                {
                    Title = "Legal Escalation Required",
                    Message = $"Agreement {board.Agreement.AgreementNo} has exceeded 90 days. Legal notice generated.",
                    NotificationType = "Alert",
                    RelatedEntity = "LegalNotice",
                    CreatedAt = now,
                    IsActive = true
                };

                _context.Notifications.Add(notification);

                count++;
            }

            if (count > 0)
            {
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation("LegalEscalationJob completed. Created {Count} legal notices.", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LegalEscalationJob failed.");
            throw;
        }
    }
}
}

