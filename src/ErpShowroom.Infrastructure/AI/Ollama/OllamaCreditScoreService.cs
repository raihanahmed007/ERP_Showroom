using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Infrastructure.AI.Ollama;

public class OllamaCreditScoreService : ICreditScoreService
{
    private readonly HttpClient _httpClient;
    private readonly string _model;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<OllamaCreditScoreService> _logger;

    public OllamaCreditScoreService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IApplicationDbContext dbContext,
        ILogger<OllamaCreditScoreService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Ollama");
        _model = configuration["AI:OllamaModel"] ?? "llama3.2";
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> CalculateCreditScoreAsync(long customerId, CancellationToken ct = default)
    {
        try
        {
            var customer = await _dbContext.Customers
                .Include(c => c.HPAgreements!)
                    .ThenInclude(a => a.EMISchedules)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId, ct);

            if (customer == null) throw new Exception($"Customer {customerId} not found");

            var monthlyIncome = customer.MonthlyIncome ?? 0;
            var pastAgreementsCount = customer.HPAgreements?.Count ?? 0;
            var latePaymentsCount = customer.HPAgreements?
                .SelectMany(a => a.EMISchedules!)
                .Count(s => s.Status == Domain.fin.Entities.EMIPaymentStatus.Overdue) ?? 0;
            
            // Check if customer has any guarantor in fin.Guarantors table.
            var hasGuarantor = await _dbContext.HPAgreements
                .AnyAsync(a => a.CustomerId == customerId && a.GuarantorId != null, ct);

            var prompt = $@"Given monthly income = {monthlyIncome}, number of past HP agreements = {pastAgreementsCount}, number of late payments = {latePaymentsCount}, guarantor = {(hasGuarantor ? "yes" : "no")}. Output ONLY a credit score between 0 and 100 (integer). Higher is better.";

            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("api/generate", requestBody, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken: ct);
            var scoreStr = result?.Response?.Trim();

            if (int.TryParse(scoreStr, out int score))
            {
                return Math.Clamp(score, 0, 100);
            }

            _logger.LogWarning("Ollama returned non-integer credit score: {ScoreStr}", scoreStr);
            return 50; // Fallback
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating credit score with Ollama for customer {CustomerId}", customerId);
            return 50; // Fallback
        }
    }

    private class OllamaResponse
    {
        public string? Response { get; set; }
    }
}
