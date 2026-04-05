using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ErpShowroom.API.HealthChecks;

public class OllamaHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OllamaHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            
            // Ollama base URL is usually from config, but checking localhost as specified in prompt requirement
            // However, it's better to check the actual configured URL. 
            // The prompt says "GET to http://localhost:11434/api/tags"
            
            var response = await client.GetAsync("http://localhost:11434/api/tags", cancellationToken);
            
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
