using System.Threading;
using System.Threading.Tasks;

namespace ErpShowroom.Application.Common.Interfaces;

public interface IOllamaService
{
    Task<string> GenerateAsync(string prompt, string model = "llama3", CancellationToken ct = default);
}

public interface ISmsService
{
    Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken ct = default);
}

public interface IWhatsAppService
{
    Task<bool> SendWhatsAppMessageAsync(string phoneNumber, string message, string mediaUrl = null, CancellationToken ct = default);
}
