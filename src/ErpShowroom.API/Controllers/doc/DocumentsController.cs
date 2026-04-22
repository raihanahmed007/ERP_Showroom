using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Serilog;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ErpShowroom.Application.doc.Commands;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.doc.Entities;

namespace ErpShowroom.API.Controllers.doc;

[Route("api/documents")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITesseractOcrService _ocrService;
    private readonly IApplicationDbContext _context;

    public DocumentsController(IMediator mediator, ITesseractOcrService ocrService, IApplicationDbContext context)
    {
        _mediator = mediator;
        _ocrService = ocrService;
        _context = context;
    }

    [HttpPost("ocr")]
    public async Task<IActionResult> ExtractOcr(IFormFile file, [FromForm] string documentType, [FromForm] long? documentId, CancellationToken ct)
    {
        // 1. Validation
        if (file == null || file.Length == 0)
            return BadRequest("File is empty.");

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest("File size exceeds 10 MB limit.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid content type. Only JPEG, PNG, and PDF are allowed.");

        try
        {
            // 2. Read bytes
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            var fileBytes = ms.ToArray();

            // 3. Call OCR Service
            var ocrResult = await _ocrService.ExtractTextAsync(fileBytes, documentType, ct);

            // 4. Database Storage
            StoredDocument? document;
            if (documentId.HasValue)
            {
                document = await _context.StoredDocuments.FindAsync(new object[] { documentId.Value }, ct);
                if (document == null) return NotFound($"Document with ID {documentId} not found.");
            }
            else
            {
                // Create new StoredDocument if not exists
                document = new StoredDocument
                {
                    FileName = file.FileName,
                    BlobPath = "pending-upload/" + Guid.NewGuid() + Path.GetExtension(file.FileName),
                    MimeType = file.ContentType,
                    FileSizeBytes = file.Length,
                    OCRText = ocrResult.RawText,
                    DocumentNo = "OCR-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                };
                _context.StoredDocuments.Add(document);
                await _context.SaveChangesAsync(ct);
            }

            // Create or update OcrDocumentData
            var ocrData = await _context.OcrDocumentDatas.FirstOrDefaultAsync(d => d.DocumentId == document.Id, ct);
            if (ocrData == null)
            {
                ocrData = new OcrDocumentData { DocumentId = document.Id };
                _context.OcrDocumentDatas.Add(ocrData);
            }

            // Map extracted fields
            ocrData.ExtractedJson = JsonConvert.SerializeObject(ocrResult.ExtractedFields);
            ocrData.Confidence = ocrResult.Confidence;
            
            if (ocrResult.ExtractedFields.ContainsKey("NidNumber")) ocrData.NidNumber = ocrResult.ExtractedFields["NidNumber"];
            if (ocrResult.ExtractedFields.ContainsKey("Name")) ocrData.Name = ocrResult.ExtractedFields["Name"];
            if (ocrResult.ExtractedFields.ContainsKey("FatherName")) ocrData.FatherName = ocrResult.ExtractedFields["FatherName"];
            if (ocrResult.ExtractedFields.ContainsKey("DateOfBirth"))
            {
                if (DateTime.TryParse(ocrResult.ExtractedFields["DateOfBirth"], out var dob)) ocrData.DateOfBirth = dob;
            }
            if (ocrResult.ExtractedFields.ContainsKey("InvoiceNumber")) ocrData.InvoiceNumber = ocrResult.ExtractedFields["InvoiceNumber"];
            if (ocrResult.ExtractedFields.ContainsKey("InvoiceAmount"))
            {
                if (decimal.TryParse(ocrResult.ExtractedFields["InvoiceAmount"], out var amount)) ocrData.InvoiceAmount = amount;
            }
            if (ocrResult.ExtractedFields.ContainsKey("InvoiceDate"))
            {
                if (DateTime.TryParse(ocrResult.ExtractedFields["InvoiceDate"], out var iDate)) ocrData.InvoiceDate = iDate;
            }

            // Low confidence check
            if (ocrResult.Confidence < 0.5f)
            {
                document.IsVerified = false;
            }

            await _context.SaveChangesAsync(ct);

            Log.Information("OCR processed for {DocumentType}. ID: {DocumentId}, Confidence: {Confidence}", 
                documentType, document.Id, ocrResult.Confidence);

            return Ok(new
            {
                document.Id,
                ocrResult.ExtractedFields,
                ocrResult.Confidence,
                IsVerified = document.IsVerified ?? true
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "OCR Processing failed", details = ex.Message });
        }
    }
}
