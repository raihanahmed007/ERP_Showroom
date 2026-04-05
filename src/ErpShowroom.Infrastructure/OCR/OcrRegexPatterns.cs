using System.Text.RegularExpressions;

namespace ErpShowroom.Infrastructure.OCR;

public static class OcrRegexPatterns
{
    // NID (Bangladesh)
    public static readonly Regex NidNumber = new(@"\b(\d{10}|\d{17})\b", RegexOptions.Compiled);
    public static readonly Regex Name = new(@"(?i)(?:mr\.|mrs\.|ms\.)?\s*([A-Z][a-z]+(?:\s+[A-Z][a-z]+)+)", RegexOptions.Compiled);
    public static readonly Regex FatherName = new(@"(?i)father\s*:\s*([A-Z][a-z]+(?:\s+[A-Z][a-z]+)+)", RegexOptions.Compiled);
    public static readonly Regex DateOfBirth = new(@"\b(\d{1,2}[/-]\d{1,2}[/-]\d{4})\b", RegexOptions.Compiled);

    // Cheque
    public static readonly Regex ChequeAmount = new(@"\b(\d+(?:,\d{3})*(?:\.\d{2})?)\b", RegexOptions.Compiled);
    public static readonly Regex ChequeDate = new(@"\b(\d{1,2}[/-]\d{1,2}[/-]\d{4})\b", RegexOptions.Compiled);
    public static readonly Regex AccountNumber = new(@"\b(\d{6,15})\b", RegexOptions.Compiled);

    // Invoice
    public static readonly Regex InvoiceNumber = new(@"(?:invoice|inv|no\.?)\s*[:\s#]*([A-Z0-9\-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex TotalAmount = new(@"(?:total|amount|due)\s*[:\s#]*(\d+(?:,\d{3})*(?:\.\d{2})?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public static readonly Regex InvoiceDate = new(@"\b(\d{1,2}[/-]\d{1,2}[/-]\d{4})\b", RegexOptions.Compiled);
}
