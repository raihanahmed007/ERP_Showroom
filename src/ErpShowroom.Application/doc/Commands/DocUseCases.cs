using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.doc.Entities;

namespace ErpShowroom.Application.doc.Commands;

public record UploadDocumentCommand(long CustomerId, long CategoryId, string FilePath) : IRequest<long>;
public class UploadDocumentHandler(IApplicationDbContext db) : IRequestHandler<UploadDocumentCommand, long> {
    public async Task<long> Handle(UploadDocumentCommand request, CancellationToken ct) {
        var doc = new StoredDocument {
            CustomerId = request.CustomerId,
            CategoryId = request.CategoryId,
            BlobPath = request.FilePath,
            FileName = System.IO.Path.GetFileName(request.FilePath),
            DocumentNo = "DOC-" + Guid.NewGuid()
        };
        db.StoredDocuments.Add(doc);
        await db.SaveChangesAsync(ct);
        return doc.Id;
    }
}

public record RunOcrCommand(long DocumentId) : IRequest<bool>;
public class RunOcrHandler(IApplicationDbContext db, IOcrService ocrService) : IRequestHandler<RunOcrCommand, bool> {
    public async Task<bool> Handle(RunOcrCommand request, CancellationToken ct) {
        var doc = await db.StoredDocuments.FirstOrDefaultAsync(d => d.Id == request.DocumentId, ct);
        if (doc == null || string.IsNullOrEmpty(doc.BlobPath)) return false;
        
        var text = ocrService.ExtractText(doc.BlobPath);
        doc.OCRText = text;
        await db.SaveChangesAsync(ct);
        return true;
    }
}
