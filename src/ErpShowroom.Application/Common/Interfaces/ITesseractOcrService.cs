using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ErpShowroom.Application.Common.Interfaces;

public interface ITesseractOcrService
{
    Task<OcrResult> ExtractTextAsync(byte[] fileBytes, string documentType, CancellationToken ct = default);
}

public class OcrResult
{
    public string RawText { get; set; } = string.Empty;
    public Dictionary<string, string> ExtractedFields { get; set; } = new();
    public float Confidence { get; set; }
}
