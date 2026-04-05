using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tesseract;
using UglyToad.PdfPig;

namespace ErpShowroom.Infrastructure.OCR;

public class TesseractOcrService : ITesseractOcrService, IDisposable
{
    private readonly Lazy<TesseractEngine> _engine;
    private readonly ILogger<TesseractOcrService> _logger;
    private readonly string _tessDataPath;
    private readonly string _languages;

    public TesseractOcrService(IConfiguration configuration, ILogger<TesseractOcrService> logger)
    {
        _logger = logger;
        var section = configuration.GetSection("Tesseract");
        _tessDataPath = section["TessDataPath"] ?? "./tessdata";
        _languages = section["Languages"] ?? "eng+ben";

        _engine = new Lazy<TesseractEngine>(() =>
        {
            try
            {
                if (!Directory.Exists(_tessDataPath))
                {
                    _logger.LogWarning("TessDataPath '{Path}' not found. OCR may fail.", _tessDataPath);
                    // Try to create it if it doesn't exist, though it needs .traineddata files
                    Directory.CreateDirectory(_tessDataPath);
                }

                var mode = Enum.TryParse<EngineMode>(section["EngineMode"] ?? "Default", out var em) ? em : EngineMode.Default;
                return new TesseractEngine(_tessDataPath, _languages, mode);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to initialize Tesseract engine at {Path}", _tessDataPath);
                throw;
            }
        });
    }

    public async Task<OcrResult> ExtractTextAsync(byte[] fileBytes, string documentType, CancellationToken ct = default)
    {
        var result = new OcrResult();
        try
        {
            bool isPdf = IsPdf(fileBytes);
            if (isPdf)
            {
                result.RawText = await ProcessPdfAsync(fileBytes, ct);
            }
            else
            {
                result.RawText = await ProcessImageAsync(fileBytes, ct);
            }

            result.ExtractedFields = ExtractFields(result.RawText, documentType);
            result.Confidence = CalculateConfidence(result.RawText, result.ExtractedFields);

            _logger.LogInformation("OCR extraction completed for {DocumentType} with confidence {Confidence}", 
                documentType, result.Confidence);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during OCR processing for document type: {DocumentType}", documentType);
            throw;
        }
    }

    private async Task<string> ProcessImageAsync(byte[] imageBytes, CancellationToken ct)
    {
        return await Task.Run(() =>
        {
            using var pix = Pix.LoadFromMemory(imageBytes);
            using var page = _engine.Value.Process(pix);
            return page.GetText();
        }, ct);
    }

    private async Task<string> ProcessPdfAsync(byte[] pdfBytes, CancellationToken ct)
    {
        return await Task.Run(() =>
        {
            var textParts = new List<string>();
            using (var document = PdfDocument.Open(pdfBytes))
            {
                foreach (var page in document.GetPages())
                {
                    // Try text extraction first (for searchable PDFs)
                    var pageText = page.Text;
                    if (string.IsNullOrWhiteSpace(pageText))
                    {
                        // For scanned PDFs, we should ideally render page to image.
                        // PdfPig doesn't render to image natively without Skia extension.
                        // For this implementation, we will log a warning or use character extraction if possible.
                        _logger.LogWarning("Page {PageNumber} appears to be a scanned PDF. Text extraction might be limited.", page.Number);
                    }
                    textParts.Add(pageText);
                }
            }
            return string.Join("\n", textParts);
        }, ct);
    }

    private bool IsPdf(byte[] bytes)
    {
        return bytes.Length > 4 &&
               bytes[0] == 0x25 && // %
               bytes[1] == 0x50 && // P
               bytes[2] == 0x44 && // D
               bytes[3] == 0x46 && // F
               bytes[4] == 0x2D;    // -
    }

    private Dictionary<string, string> ExtractFields(string rawText, string documentType)
    {
        var fields = new Dictionary<string, string>();

        switch (documentType.ToUpper())
        {
            case "NID":
                fields["NidNumber"] = OcrRegexPatterns.NidNumber.Match(rawText).Value;
                fields["Name"] = OcrRegexPatterns.Name.Match(rawText).Groups[1].Value.Trim();
                fields["FatherName"] = OcrRegexPatterns.FatherName.Match(rawText).Groups[1].Value.Trim();
                fields["DateOfBirth"] = OcrRegexPatterns.DateOfBirth.Match(rawText).Value;
                break;

            case "CHEQUE":
                var amountMatches = OcrRegexPatterns.ChequeAmount.Matches(rawText);
                if (amountMatches.Count > 0)
                {
                    // Take the largest numeric match as amount usually
                    fields["Amount"] = amountMatches.Cast<Match>()
                        .OrderByDescending(m => m.Value.Length)
                        .First().Value;
                }
                fields["Date"] = OcrRegexPatterns.ChequeDate.Match(rawText).Value;
                fields["AccountNumber"] = OcrRegexPatterns.AccountNumber.Match(rawText).Value;
                break;

            case "INVOICE":
                fields["InvoiceNumber"] = OcrRegexPatterns.InvoiceNumber.Match(rawText).Groups[1].Value.Trim();
                fields["InvoiceAmount"] = OcrRegexPatterns.TotalAmount.Match(rawText).Groups[1].Value.Trim();
                fields["InvoiceDate"] = OcrRegexPatterns.InvoiceDate.Match(rawText).Value;
                break;
        }

        return fields;
    }

    private float CalculateConfidence(string rawText, Dictionary<string, string> fields)
    {
        if (string.IsNullOrWhiteSpace(rawText)) return 0;
        
        // Simple heuristic: ratio of extracted fields to expected fields
        int expected = fields.Count;
        int found = fields.Values.Count(v => !string.IsNullOrWhiteSpace(v));
        
        if (expected == 0) return 0.5f;

        // Start with a base confidence and modify based on matches
        float baseConfidence = (float)found / expected;
        
        // Tesseract's own confidence could be used if we were processing single images,
        // but here we combine results. 
        return baseConfidence;
    }

    public void Dispose()
    {
        if (_engine.IsValueCreated)
        {
            _engine.Value.Dispose();
        }
    }
}
