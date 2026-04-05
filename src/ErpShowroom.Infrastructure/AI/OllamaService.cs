using OllamaSharp;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.AI;

public class OllamaService(OllamaApiClient client) : IOllamaService
{
    public async Task<string> GenerateAsync(string prompt, string model = ""llama3"", CancellationToken ct = default)
    {
        var result = new System.Text.StringBuilder();
        await foreach (var chunk in client.GenerateAsync(prompt, model).WithCancellation(ct))
        {
            result.Append(chunk?.Response);
        }
        return result.ToString();
    }
}
