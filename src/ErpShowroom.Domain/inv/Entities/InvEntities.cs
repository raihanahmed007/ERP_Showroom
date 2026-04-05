using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.inv.Entities;

public class Product : BaseEntity
{
    [Required, MaxLength(50)] public string? ProductCode { get; set; }
    [Required, MaxLength(200)] public string? ProductName { get; set; }
    public long? CategoryId { get; set; }
    public long? BrandId { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; } = "kg";
    public int? WarrantyMonths { get; set; } = 12;
    public bool? IsSerialized { get; set; } = true;
    public string? Model { get; set; }
    public string? Color { get; set; }
    public int? ReorderLevel { get; set; }
    public int? ReorderQuantity { get; set; }
    public string? ProductImageUrl { get; set; }
    public string? Description { get; set; }
    public decimal? VATPct { get; set; } = 15;
    public decimal? DiscountLimitPct { get; set; } = 10;
    public string? Barcode { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual ProductCategory? Category { get; set; }

    [ForeignKey(nameof(BrandId))]
    public virtual Brand? Brand { get; set; }

    public virtual ICollection<SerialNumber>? Serials { get; set; }
    public virtual ICollection<StockBalance>? StockBalances { get; set; }
}

public class ProductCategory : BaseEntity
{
    [Required, MaxLength(100)] public string? CategoryName { get; set; }
    public long? ParentCategoryId { get; set; }

    [ForeignKey(nameof(ParentCategoryId))]
    public virtual ProductCategory? ParentCategory { get; set; }

    [InverseProperty(nameof(ParentCategory))]
    public virtual ICollection<ProductCategory>? ChildCategories { get; set; }
}

public class Brand : BaseEntity
{
    [Required, MaxLength(100)] public string? BrandName { get; set; }
    public string? LogoUrl { get; set; }
}

public class SerialNumber : BaseEntity
{
    public long? ProductId { get; set; }
    [Required, MaxLength(100)] public string? SerialNo { get; set; }
    [MaxLength(200)] public string? QRCode { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public SerialStatusEnum? Status { get; set; } = SerialStatusEnum.InStock;
    public DateTime? LastTransactionDate { get; set; }
    public long? CurrentHPAgreementId { get; set; }
    public decimal? PurchaseCost { get; set; }
    public decimal? SellingPrice { get; set; }
    public string? BatchNo { get; set; }
    public long? WarehouseLocationId { get; set; }
    public decimal? LastKnownLatitude { get; set; }
    public decimal? LastKnownLongitude { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }

    [ForeignKey(nameof(WarehouseLocationId))]
    public virtual WarehouseLocation? WarehouseLocation { get; set; }
}

public class StockBalance : BaseEntity
{
    public long? ProductId { get; set; }
    public int? QuantityOnHand { get; set; } = 0;
    public int? QuantityReserved { get; set; } = 0;
    public int? QuantityOrdered { get; set; } = 0;
    public int? ReorderLevel { get; set; }
    public int? ReorderQuantity { get; set; }
    public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }

    /// <summary>
    /// Reserves the specified quantity of stock.
    /// </summary>
    /// <param name="qty">Quantity to reserve.</param>
    public void Reserve(int qty)
    {
        if (qty <= 0) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Quantity must be greater than zero.");
        if (QuantityOnHand < qty) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Insufficient quantity on hand.");

        QuantityOnHand -= qty;
        QuantityReserved += qty;
        LastUpdated = DateTime.UtcNow;

        AddDomainEvent(new ErpShowroom.Domain.inv.Events.StockReservedEvent(ProductId ?? 0, qty, DateTime.UtcNow));
    }

    /// <summary>
    /// Releases the specified quantity of reserved stock back to on-hand.
    /// </summary>
    /// <param name="qty">Quantity to release.</param>
    public void Release(int qty)
    {
        if (qty <= 0) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Quantity must be greater than zero.");
        if (QuantityReserved < qty) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Insufficient reserved quantity.");

        QuantityReserved -= qty;
        QuantityOnHand += qty;
        LastUpdated = DateTime.UtcNow;

        AddDomainEvent(new ErpShowroom.Domain.inv.Events.StockReleasedEvent(ProductId ?? 0, qty, DateTime.UtcNow));
    }

    /// <summary>
    /// Ships the specified quantity of reserved stock (deducts from reserved but does not return to on-hand).
    /// </summary>
    /// <param name="qty">Quantity to ship.</param>
    public void Ship(int qty)
    {
        if (qty <= 0) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Quantity must be greater than zero.");
        if (QuantityReserved < qty) throw new ErpShowroom.Domain.Common.Exceptions.DomainException("Insufficient reserved quantity to ship.");

        QuantityReserved -= qty;
        LastUpdated = DateTime.UtcNow;

        AddDomainEvent(new ErpShowroom.Domain.inv.Events.StockShippedEvent(ProductId ?? 0, qty, DateTime.UtcNow));
    }
}

public class StockTransfer : BaseEntity
{
    [Required, MaxLength(50)] public string? TransferNo { get; set; }
    public long? FromBranchId { get; set; }
    public long? ToBranchId { get; set; }
    public DateTime? TransferDate { get; set; }
    public TransferStatusEnum? Status { get; set; } = TransferStatusEnum.Pending;
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Remarks { get; set; }
    public long? ReceivedByUserId { get; set; }
    public DateTime? ReceivedAt { get; set; }

    public virtual ICollection<TransferDetail>? TransferDetails { get; set; }
}

public class TransferDetail : BaseEntity
{
    public long? TransferId { get; set; }
    public long? SerialId { get; set; }
    public long? ProductId { get; set; }

    [ForeignKey(nameof(TransferId))]
    public virtual StockTransfer? Transfer { get; set; }

    [ForeignKey(nameof(SerialId))]
    public virtual SerialNumber? Serial { get; set; }
}

public class StockAging : BaseEntity
{
    public long? ProductId { get; set; }
    public int? DaysInStock { get; set; }
    public int? Quantity { get; set; }
    public DateTime? AsOfDate { get; set; }
}

public class WarehouseLocation : BaseEntity
{
    [Required, MaxLength(20)] public string? LocationCode { get; set; }
    [Required, MaxLength(100)] public string? LocationName { get; set; }
    public string? ShelfNo { get; set; }
    public string? RowNo { get; set; }
}
