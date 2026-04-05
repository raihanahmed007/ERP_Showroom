using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.AI.DTOs;
using ErpShowroom.Domain.crm.Entities;

namespace ErpShowroom.Infrastructure.AI.Ollama;

public class OllamaSentimentService : ISentimentService
{
    private readonly HttpClient _httpClient;
    private readonly string _model;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<OllamaSentimentService> _logger;

    public OllamaSentimentService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IApplicationDbContext dbContext,
        ILogger<OllamaSentimentService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Ollama");
        _model = configuration["AI:OllamaModel"] ?? "llama3.2";
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<SentimentResult> AnalyzeSentimentAsync(long customerId, string conversationText, CancellationToken ct = default)
    {
        var prompt = $@"Analyze the sentiment of the following customer conversation. Respond only with valid JSON: {{""sentiment"": ""positive|negative|neutral"", ""score"": 0.0 to 1.0}}. Conversation: {conversationText}";

        try
        {
            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false,
                format = "json"
            };

            var response = await _httpClient.PostAsJsonAsync("api/generate", requestBody, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken: ct);
            var sentimentJson = result?.Response?.Trim();

            if (string.IsNullOrEmpty(sentimentJson)) throw new Exception("Empty response from Ollama");

            var sentimentData = JsonSerializer.Deserialize<SentimentJsonResponse>(sentimentJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var log = new AISentimentLog
            {
                CustomerId = customerId,
                ConversationText = conversationText,
                SentimentScore = sentimentData?.Score ?? 0.5m,
                SentimentLabel = sentimentData?.Sentiment ?? "neutral",
                ConversationDate = DateTime.UtcNow
            };

            _dbContext.AISentimentLogs.Add(log);
            await _dbContext.SaveChangesAsync(ct);

            return new SentimentResult
            {
                Sentiment = log.SentimentLabel,
                Score = log.SentimentScore ?? 0.5m,
                LogId = log.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing sentiment with Ollama for customer {CustomerId}", customerId);
            return new SentimentResult { Sentiment = "neutral", Score = 0.5m };
        }
    }

    private class OllamaResponse
    {
        public string? Response { get; set; }
    }

    private class SentimentJsonResponse
    {
        public string? Sentiment { get; set; }
        public decimal Score { get; set; }
    }
}
