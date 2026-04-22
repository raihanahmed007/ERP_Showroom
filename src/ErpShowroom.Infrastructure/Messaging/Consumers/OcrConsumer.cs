using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using ErpShowroom.Domain.Common.Events;
using ErpShowroom.Domain.doc.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.Infrastructure.Messaging.Consumers
{
public class OcrConsumer : IConsumer<DocumentProcessedEvent>
{
    private readonly ILogger<OcrConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public OcrConsumer(ILogger<OcrConsumer> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task Consume(ConsumeContext<DocumentProcessedEvent> context)
    {
        var ev = context.Message;
        
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // ITesseractService usage: using dynamic fallback or local definition if not fully wired in previous tasks
        var tesseractService = scope.ServiceProvider.GetService<OCR.ITesseractService>();
        
        string ocrJson = string.Empty;

        if (tesseractService != null && !string.IsNullOrEmpty(ev.BlobPath))
        {
            ocrJson = await tesseractService.ExtractTextAsync(ev.BlobPath);
        }
        else
        {
            // Fallback simulation based on event instruction
            ocrJson = ev.OcrDataJson ?? "{ \"info\": \"mock extracted\" }";
        }

        // Check Idempotency based on DocumentId
        bool alreadyProcessed = await dbContext.Set<OcrDocumentData>()
            .AnyAsync(o => o.DocumentId == ev.DocumentId);

        if (alreadyProcessed)
        {
            _logger.LogInformation("Document {DocumentId} already OCR processed.", ev.DocumentId);
            return;
        }

        var ocrData = new OcrDocumentData
        {
            DocumentId = ev.DocumentId,
            ExtractedJson = ocrJson,
            Confidence = (float?)ev.Confidence,
            IsActive = true
        };

        dbContext.Set<OcrDocumentData>().Add(ocrData);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("OCR processed for document {DocumentId}.", ev.DocumentId);
    }
}

}

namespace ErpShowroom.Infrastructure.OCR
{
    public interface ITesseractService
    {
        Task<string> ExtractTextAsync(string imagePath);
    }
}
