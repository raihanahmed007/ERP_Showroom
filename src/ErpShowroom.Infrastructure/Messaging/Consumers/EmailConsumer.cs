using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MassTransit;
using ErpShowroom.Domain.Common.Events;

namespace ErpShowroom.Infrastructure.Messaging.Consumers;

public class EmailConsumer : IConsumer<AgreementCreatedEvent>
{
    private readonly ILogger<EmailConsumer> _logger;

    public EmailConsumer(ILogger<EmailConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AgreementCreatedEvent> context)
    {
        var ev = context.Message;

        // Ensure idempotence: check if email already sent by keeping a record locally.
        // E.g., AppDbContext.Notifications.Any(n => n.MessageId == ev.CorrelationId)

        if (string.IsNullOrWhiteSpace(ev.CustomerEmail))
        {
            _logger.LogWarning("No email provided for Agreement {AgreementNo}.", ev.AgreementNo);
            return;
        }

        try
        {
            using var client = new System.Net.Mail.SmtpClient("localhost", 25);
            using var message = new MailMessage("noreply@himumotors.com", ev.CustomerEmail)
            {
                Subject = "Your Hire Purchase Agreement is Ready",
                Body = $"Dear Customer,\n\nYour agreement ({ev.AgreementNo}) has been approved.\n\nThank you for choosing Himu Motors."
            };

            await client.SendMailAsync(message);
            _logger.LogInformation("Email successfully sent to {Email} for Agreement {AgreementNo}.", ev.CustomerEmail, ev.AgreementNo);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}.", ev.CustomerEmail);
            throw; // Trigger MassTransit retry
        }
    }
}
