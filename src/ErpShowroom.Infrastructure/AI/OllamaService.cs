using OllamaSharp;
using OllamaSharp.Models;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.AI;

public class OllamaService(OllamaApiClient client) : IOllamaService
{
    public async Task<string> GenerateAsync(string prompt, string model = "llama3", CancellationToken ct = default)
    {
        var result = new System.Text.StringBuilder();
        var request = new GenerateRequest
        {
            Prompt = prompt,
            Model = model
        };

        await foreach (var chunk in client.GenerateAsync(request, ct).WithCancellation(ct))
        {
            result.Append(chunk?.Response);
        }
        return result.ToString();
    }
}
