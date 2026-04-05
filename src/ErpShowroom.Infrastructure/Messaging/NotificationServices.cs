using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.Messaging;

public class SmsService : ISmsService
{
    public Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken ct = default)
    {
        // Integration point for Twilio or other SMS provider
        return Task.FromResult(true);
    }
}

public class WhatsAppService : IWhatsAppService
{
    public Task<bool> SendWhatsAppMessageAsync(string phoneNumber, string message, string mediaUrl = null, CancellationToken ct = default)
    {
        // Integration point for WhatsApp Business API
        return Task.FromResult(true);
    }
}
