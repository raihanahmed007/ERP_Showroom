using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.prc.Entities;

public class Supplier : BaseEntity
{
    [Required, MaxLength(50)] public string? SupplierCode { get; set; }
    [Required, MaxLength(200)] public string? SupplierName { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? TINNumber { get; set; }
    public string? BinNumber { get; set; }
    public decimal? CreditLimit { get; set; }
    public int? LeadTimeDays { get; set; }
    public bool? IsActive { get; set; } = true;
}

public class Vendor : BaseEntity
{
    [Required, MaxLength(50)] public string? VendorCode { get; set; }
    [Required, MaxLength(200)] public string? VendorName { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal? CreditLimit { get; set; }
    public string? PaymentTerms { get; set; }
    public string? TINNumber { get; set; }
    public string? BinNumber { get; set; }
    public int? LeadTimeDays { get; set; }
    public decimal? AverageRating { get; set; }

    public virtual ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
    public virtual ICollection<SupplierLedger>? SupplierLedgers { get; set; }
}

public class PurchaseOrder : BaseEntity
{
    [Required, MaxLength(50)] public string? PONumber { get; set; }
    public long? VendorId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public POStatus? Status { get; set; } = POStatus.Draft;
    public decimal? TotalAmount { get; set; }
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ShippingAddress { get; set; }
    public string? TermsAndConditions { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? ShippingCost { get; set; }

    [ForeignKey(nameof(VendorId))]
    public virtual Vendor? Vendor { get; set; }

    public virtual ICollection<PODetail>? PODetails { get; set; }
    public virtual ICollection<GRN>? GRNs { get; set; }
}

public class PODetail : BaseEntity
{
    public long? PONo { get; set; }
    public long? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitCost { get; set; }
    public int? ReceivedQuantity { get; set; } = 0;

    [ForeignKey(nameof(PONo))]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
}

public class GRN : BaseEntity
{
    [Required, MaxLength(50)] public string? GRNNumber { get; set; }
    public long? PONo { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public long? ReceivedByUserId { get; set; }
    public GRNStatus? Status { get; set; } = GRNStatus.Draft;
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string? ChallanNo { get; set; }
    public string? WarehouseLocation { get; set; }

    [ForeignKey(nameof(PONo))]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public virtual ICollection<GRNDetail>? GRNDetails { get; set; }
    public virtual ICollection<LandedCost>? LandedCosts { get; set; }
}

public class GRNDetail : BaseEntity
{
    public long? GRNId { get; set; }
    public long? ProductId { get; set; }
    public string? SerialNo { get; set; }
    public int? QuantityReceived { get; set; }
    public decimal? UnitCostApplied { get; set; }
    public string? BatchNo { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    [ForeignKey(nameof(GRNId))]
    public virtual GRN? GRN { get; set; }
}

public class LandedCost : BaseEntity
{
    public long? GRNId { get; set; }
    public string? CostType { get; set; } // Freight, Insurance, Tax, Handling
    public decimal? Amount { get; set; }
    public string? AllocationMethod { get; set; } // ByQuantity, ByValue, ByWeight

    [ForeignKey(nameof(GRNId))]
    public virtual GRN? GRN { get; set; }
}

public class SupplierLedger : BaseEntity
{
    public long? SupplierId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? VoucherType { get; set; }
    public long? VoucherId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Balance { get; set; }
    public string? Remarks { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier? Supplier { get; set; }
}
