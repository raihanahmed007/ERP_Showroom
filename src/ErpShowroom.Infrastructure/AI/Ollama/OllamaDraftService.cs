using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.AI.Ollama;

public class OllamaDraftService : IDraftService
{
    private readonly HttpClient _httpClient;
    private readonly string _model;
    private readonly ILogger<OllamaDraftService> _logger;

    public OllamaDraftService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<OllamaDraftService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Ollama");
        _model = configuration["AI:OllamaModel"] ?? "llama3.2";
        _logger = logger;
    }

    public async Task<string> DraftReminderAsync(int overdueDays, string customerName, decimal outstandingAmount, CancellationToken ct = default)
    {
        var prompt = $@"Write a polite SMS reminder for a customer named {customerName} whose payment is {overdueDays} days overdue. Outstanding amount: {outstandingAmount} BDT. Keep under 160 characters for SMS.";

        try
        {
            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("api/generate", requestBody, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken: ct);
            return result?.Response?.Trim().Replace("\"", "") ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error drafting reminder for {CustomerName}", customerName);
            return $"Reminder: Your payment of {outstandingAmount} BDT is {overdueDays} days overdue. Please pay soon.";
        }
    }

    private class OllamaResponse
    {
        public string? Response { get; set; }
    }
}
