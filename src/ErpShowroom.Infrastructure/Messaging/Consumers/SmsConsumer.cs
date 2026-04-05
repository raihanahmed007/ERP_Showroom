using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MassTransit;
using ErpShowroom.Domain.Common.Events;

namespace ErpShowroom.Infrastructure.Messaging.Consumers;

public class SmsConsumer :
    IConsumer<AgreementCreatedEvent>,
    IConsumer<PaymentReceivedEvent>
{
    private readonly ILogger<SmsConsumer> _logger;

    public SmsConsumer(ILogger<SmsConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AgreementCreatedEvent> context)
    {
        var ev = context.Message;
        
        // Ensure consumer is idempotent - checking external DB or log to verify duplicate processing 
        // For example purposes we log out it explicitly
        _logger.LogInformation("SMS to {CustomerPhone}: Your agreement {AgreementNo} is approved.", ev.CustomerPhone, ev.AgreementNo);

        // Placeholder for future Twilio integration
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<PaymentReceivedEvent> context)
    {
        var ev = context.Message;
        
        _logger.LogInformation("SMS: Received payment of {Amount} for Agreement {AgreementId}.", ev.Amount, ev.HPAgreementId);

        await Task.CompletedTask;
    }
}
