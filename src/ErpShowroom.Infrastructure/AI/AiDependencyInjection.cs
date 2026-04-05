using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Infrastructure.AI.Ollama;

namespace ErpShowroom.Infrastructure.AI;

public static class AiDependencyInjection
{
    public static IServiceCollection AddOllamaServices(this IServiceCollection services, IConfiguration configuration)
    {
        var aiConfig = configuration.GetSection("AI");
        var baseUrl = aiConfig["OllamaBaseUrl"] ?? "http://localhost:11434";
        var timeoutSeconds = int.Parse(aiConfig["TimeoutSeconds"] ?? "30");

        services.AddHttpClient("Ollama", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        });

        services.AddScoped<ISentimentService, OllamaSentimentService>();
        services.AddScoped<IDraftService, OllamaDraftService>();
        services.AddScoped<ICreditScoreService, OllamaCreditScoreService>();

        return services;
    }
}
