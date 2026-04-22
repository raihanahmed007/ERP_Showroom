using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MassTransit;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.Common.Events;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.fin.Commands;

public class CreateAgreementCommand
{
    public long CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CreateAgreementCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateAgreementCommandHandler> _logger;

    public CreateAgreementCommandHandler(
        IApplicationDbContext context, 
        IPublishEndpoint publishEndpoint,
        ILogger<CreateAgreementCommandHandler> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<long> Handle(CreateAgreementCommand request, CancellationToken cancellationToken)
    {
        // 1. Execute Domain Logic
        var agreement = new HPAgreement
        {
            CustomerId = request.CustomerId,
            AgreementNo = $"HP-{DateTime.UtcNow:yyyyMMdd}-{request.CustomerId}-{Guid.NewGuid().ToString().Substring(0, 4)}",
            FinanceAmount = request.TotalAmount,
            Status = HPAgreementStatus.Active,
            IsActive = true
        };

        _context.HPAgreements.Add(agreement);

        // 2. Save implicitly grouping operations. 
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Publish Event via MassTransit IPublishEndpoint. 
        // The InMemoryOutbox acts as an interception scoped inside DI container ensuring safe dispatch if multiple exist or failures happen gracefully.
        var integrationEvent = new AgreementCreatedEvent
        {
            HPAgreementId = agreement.Id,
            CustomerId = request.CustomerId,
            AgreementNo = agreement.AgreementNo,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone
        };

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        
        _logger.LogInformation("Agreement {AgreementNo} created and AgreementCreatedEvent submitted to MassTransit.", agreement.AgreementNo);

        return agreement.Id;
    }
}
