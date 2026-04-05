using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.crm.Commands;

public record CreateCustomerCommand(string FullName, string Phone, string NIDNumber) : IRequest<long>;
public class CreateCustomerHandler(IApplicationDbContext db) : IRequestHandler<CreateCustomerCommand, long> {
    public async Task<long> Handle(CreateCustomerCommand request, CancellationToken ct) {
        var customer = new Customer { 
            FullName = request.FullName, 
            Phone = request.Phone, 
            NIDNumber = request.NIDNumber,
            CustomerCode = "CUST-" + Guid.NewGuid()
        };
        db.Customers.Add(customer);
        await db.SaveChangesAsync(ct);
        return customer.Id;
    }
}

public record ConvertLeadToCustomerCommand(long LeadId) : IRequest<long?>;
public class ConvertLeadToCustomerHandler(IApplicationDbContext db) : IRequestHandler<ConvertLeadToCustomerCommand, long?> {
    public async Task<long?> Handle(ConvertLeadToCustomerCommand request, CancellationToken ct) {
        var lead = await db.Leads.FirstOrDefaultAsync(l => l.Id == request.LeadId, ct);
        if (lead == null) return null;
        
        var customer = new Customer { 
            FullName = lead.CustomerName ?? "Unknown", 
            Phone = lead.Phone ?? "", 
            CustomerCode = "CUST-" + Guid.NewGuid()
        };
        
        db.Customers.Add(customer);
        lead.LeadStatus = LeadStatusEnum.Converted;
        lead.ConvertedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(ct);
        lead.ConvertedToCustomerId = customer.Id;
        await db.SaveChangesAsync(ct);
        
        return customer.Id;
    }
}

public record LogSentimentCommand(long CustomerId, string ConversationText) : IRequest<long>;
public class LogSentimentHandler(IApplicationDbContext db) : IRequestHandler<LogSentimentCommand, long> {
    public async Task<long> Handle(LogSentimentCommand request, CancellationToken ct) {
        var sentiment = new AISentimentLog {
            CustomerId = request.CustomerId,
            ConversationText = request.ConversationText,
            ConversationDate = DateTime.UtcNow
        };
        db.AISentimentLogs.Add(sentiment);
        await db.SaveChangesAsync(ct);
        return sentiment.Id;
    }
}
