using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.Common.Constants;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.fin.Queries;

public class GetAgreementQuery
{
    public long CustomerId { get; set; }
}

public class GetAgreementQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public GetAgreementQueryHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<HPAgreement?> Handle(GetAgreementQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = CacheKeys.EmiCustomer(request.CustomerId);

        // 1. Try get from cache
        var cachedAgreement = await _cacheService.GetAsync<HPAgreement>(cacheKey, cancellationToken);
        
        if (cachedAgreement != null)
        {
            return cachedAgreement; // Cache hit
        }

        // 2. Cache miss, fall back to DB
        // Using AsNoTracking since this is a read query
        var agreement = await _context.HPAgreements
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.CustomerId == request.CustomerId && a.IsActive == true, cancellationToken);

        // 3. Set into cache if found
        if (agreement != null)
        {
            // Explicitly setting cache expiration to 15 mins absolute, 5 mins sliding for demo
            await _cacheService.SetAsync(
                cacheKey, 
                agreement, 
                absoluteExpiration: TimeSpan.FromMinutes(15), 
                slidingExpiration: TimeSpan.FromMinutes(5), 
                ct: cancellationToken);
        }

        return agreement;
    }
}
