using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.inv.Commands;

public record CreateProductCommand(string ProductName, string ProductCode) : IRequest<long>;
public class CreateProductHandler(IApplicationDbContext db) : IRequestHandler<CreateProductCommand, long> {
    public async Task<long> Handle(CreateProductCommand request, CancellationToken ct) {
        var product = new Product { ProductName = request.ProductName, ProductCode = request.ProductCode };
        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
        return product.Id;
    }
}

public record TransferStockCommand(List<long> SerialIds, long FromBranchId, long ToBranchId) : IRequest<long>;
public class TransferStockHandler(IApplicationDbContext db) : IRequestHandler<TransferStockCommand, long> {
    public async Task<long> Handle(TransferStockCommand request, CancellationToken ct) {
        var tx = new StockTransfer { 
            FromBranchId = request.FromBranchId, 
            ToBranchId = request.ToBranchId, 
            TransferDate = DateTime.UtcNow, 
            TransferNo = "TX-" + Guid.NewGuid() 
        };
        db.StockTransfers.Add(tx);
        await db.SaveChangesAsync(ct);
        return tx.Id;
    }
}

public record ReceiveGRNCommand(long GRNId) : IRequest<bool>;
public class ReceiveGRNHandler(IApplicationDbContext db) : IRequestHandler<ReceiveGRNCommand, bool> {
    public async Task<bool> Handle(ReceiveGRNCommand request, CancellationToken ct) {
        var grn = await db.GRNs.FirstOrDefaultAsync(g => g.Id == request.GRNId, ct);
        if (grn != null) { 
            grn.Status = ErpShowroom.Domain.Common.GRNStatus.Completed; 
            await db.SaveChangesAsync(ct); 
            return true; 
        }
        return false;
    }
}

public record GetSerialStatusQuery(string SerialNo) : IRequest<SerialNumber?>;
public class GetSerialStatusHandler(IApplicationDbContext db) : IRequestHandler<GetSerialStatusQuery, SerialNumber?> {
    public async Task<SerialNumber?> Handle(GetSerialStatusQuery request, CancellationToken ct) {
        return await db.SerialNumbers.FirstOrDefaultAsync(s => s.SerialNo == request.SerialNo, ct);
    }
}
