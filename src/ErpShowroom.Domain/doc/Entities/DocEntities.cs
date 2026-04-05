using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.crm.Entities;

namespace ErpShowroom.Domain.doc.Entities;

public class DocumentCategory : BaseEntity
{
    [Required, MaxLength(100)] public string? CategoryName { get; set; }
    public int? RetentionYears { get; set; } = 7;
    public bool? RequiresLegalHold { get; set; } = false;
    public string? AllowedMimeTypes { get; set; }
    public long? MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024;
}

public class StoredDocument : BaseEntity
{
    public long? CustomerId { get; set; }
    public long? CategoryId { get; set; }
    public string? DocumentNo { get; set; }
    [Required, MaxLength(200)] public string? FileName { get; set; }
    [Required, MaxLength(500)] public string? BlobPath { get; set; }
    public string? OCRText { get; set; }
    public long? UploadedByUserId { get; set; }
    public bool? IsUnderLegalHold { get; set; } = false;
    public DateTime? ExpiryDate { get; set; }
    public string? FileHash { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? MimeType { get; set; }
    public bool? IsVerified { get; set; } = false;
    public long? VerifiedByUserId { get; set; }
    public DateTime? VerifiedAt { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual DocumentCategory? Category { get; set; }

    public virtual ICollection<LegalHold>? LegalHolds { get; set; }
}

public class LegalHold : BaseEntity
{
    public long? DocumentId { get; set; }
    public string? HoldReason { get; set; }
    public long? PlacedByUserId { get; set; }
    public DateTime? PlacedAt { get; set; } = DateTime.UtcNow;
    public long? ReleasedByUserId { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public string? ReleaseReason { get; set; }

    [ForeignKey(nameof(DocumentId))]
    public virtual StoredDocument? Document { get; set; }
}

public class OcrDocumentData : BaseEntity
{
    public long? DocumentId { get; set; }
    public string? ExtractedJson { get; set; }
    public string? NidNumber { get; set; }
    public string? Name { get; set; }
    public string? FatherName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal? InvoiceAmount { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public float? Confidence { get; set; }

    [ForeignKey(nameof(DocumentId))]
    public virtual StoredDocument? Document { get; set; }
}
