using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.Common.Constants;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.fin.Commands;

public class ApplyPaymentCommand
{
    public long CustomerId { get; set; }
    public long AgreementId { get; set; }
    public decimal Amount { get; set; }
}

public class ApplyPaymentCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public ApplyPaymentCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(ApplyPaymentCommand request, CancellationToken cancellationToken)
    {
        var agreement = await _context.HPAgreements
            .FirstOrDefaultAsync(a => a.Id == request.AgreementId && a.CustomerId == request.CustomerId, cancellationToken);

        if (agreement == null)
            return false;

        // Perform payment specific business logic
        var payment = new Payment
        {
            HPAgreementId = agreement.Id,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            IsActive = true
        };

        _context.Payments.Add(payment);

        // Recalculate agreement balances, etc...
        
        await _context.SaveChangesAsync(cancellationToken);

        // ----------------------------------------------------
        // Cache Invalidation
        // After a payment is made, remove the customer's EMI cache
        // ----------------------------------------------------
        string cacheKey = CacheKeys.EmiCustomer(request.CustomerId);
        await _cacheService.RemoveAsync(cacheKey, cancellationToken);

        return true;
    }
}
