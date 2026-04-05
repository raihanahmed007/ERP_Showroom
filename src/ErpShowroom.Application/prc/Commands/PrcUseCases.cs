using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.prc.Commands;

public record PODetailDto(long ProductId, int Quantity, decimal UnitCost);

public record CreatePurchaseOrderCommand(long VendorId, DateTime OrderDate, DateTime ExpectedDeliveryDate, List<PODetailDto> Details) : IRequest<long>;
public class CreatePurchaseOrderHandler(IApplicationDbContext db) : IRequestHandler<CreatePurchaseOrderCommand, long> {
    public async Task<long> Handle(CreatePurchaseOrderCommand request, CancellationToken ct) {
        var po = new PurchaseOrder {
            VendorId = request.VendorId,
            OrderDate = request.OrderDate,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            PONumber = "PO-" + Guid.NewGuid(),
            Status = POStatus.Draft,
            TotalAmount = request.Details.Sum(d => d.Quantity * d.UnitCost)
        };
        db.PurchaseOrders.Add(po);
        po.PODetails = request.Details.Select(d => new PODetail { ProductId = d.ProductId, Quantity = d.Quantity, UnitCost = d.UnitCost }).ToList();
        await db.SaveChangesAsync(ct);
        return po.Id;
    }
}

public record ReceiveGoodsCommand(long PONo, string InvoiceNo) : IRequest<long>;
public class ReceiveGoodsHandler(IApplicationDbContext db) : IRequestHandler<ReceiveGoodsCommand, long> {
    public async Task<long> Handle(ReceiveGoodsCommand request, CancellationToken ct) {
        var grn = new GRN { PONo = request.PONo, GRNNumber = "GRN-" + Guid.NewGuid(), ReceivedDate = DateTime.UtcNow, InvoiceNo = request.InvoiceNo };
        db.GRNs.Add(grn);
        await db.SaveChangesAsync(ct);
        return grn.Id;
    }
}
