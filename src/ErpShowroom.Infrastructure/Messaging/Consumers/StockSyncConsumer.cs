using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using ErpShowroom.Domain.Common.Events;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.Messaging.Consumers;

public class StockSyncConsumer : IConsumer<SerialTransferredEvent>
{
    private readonly ILogger<StockSyncConsumer> _logger;
    private readonly IApplicationDbContext _context;

    public StockSyncConsumer(ILogger<StockSyncConsumer> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Consume(ConsumeContext<SerialTransferredEvent> context)
    {
        var ev = context.Message;

        // Idempotency Check: if you maintain internal logs or logic. We assume RowVersion concurrency catches replays appropriately or explicit flags.
        // E.g., AppDbContext.IntegrationLogs.Any(...)

        try
        {
            var sourceBalance = await _context.StockBalances
                .FirstOrDefaultAsync(b => b.ProductId == ev.ProductId && b.BranchId == ev.FromBranchId, context.CancellationToken);

            var destBalance = await _context.StockBalances
                .FirstOrDefaultAsync(b => b.ProductId == ev.ProductId && b.BranchId == ev.ToBranchId, context.CancellationToken);

            if (sourceBalance != null && sourceBalance.QuantityOnHand > 0)
            {
                sourceBalance.QuantityOnHand -= 1;
            }

            if (destBalance != null)
            {
                destBalance.QuantityOnHand += 1;
            }
            else
            {
                destBalance = new Domain.inv.Entities.StockBalance
                {
                    ProductId = ev.ProductId,
                    BranchId = ev.ToBranchId,
                    QuantityOnHand = 1,
                    IsActive = true
                };
                _context.StockBalances.Add(destBalance);
            }

            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Stock synchronized for Serial {SerialId} from Branch {FromBranch} to {ToBranch}.", 
                ev.SerialId, ev.FromBranchId, ev.ToBranchId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict during stock sync for Serial {SerialId}. Retrying...", ev.SerialId);
            throw; // Will be safely retried by MassTransit's extensive Retry policy configured in AddMassTransitWithRabbitMq
        }
    }
}
