using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ErpShowroom.API.HealthChecks;

public class OllamaHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OllamaHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var baseUrl = _configuration["AI:OllamaBaseUrl"] ?? "http://localhost:11434";
            var client = _httpClientFactory.CreateClient("Ollama");
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/tags", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Ollama is reachable and responding with 200 OK");
            }

            return HealthCheckResult.Unhealthy($"Ollama returned status {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Ollama check failed: {ex.Message}");
        }
    }
}
