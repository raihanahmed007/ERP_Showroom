using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace ErpShowroom.API.HealthChecks;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public RabbitMqHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration["RabbitMQ:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var host = _configuration["RabbitMQ:Host"] ?? "localhost";
            var port = _configuration.GetValue<int?>("RabbitMQ:Port") ?? 5672;
            var vhost = _configuration["RabbitMQ:VHost"] ?? "/";
            if (!vhost.StartsWith("/", StringComparison.Ordinal))
            {
                vhost = "/" + vhost;
            }
            var user = _configuration["RabbitMQ:User"] ?? "guest";
            var pass = _configuration["RabbitMQ:Pass"] ?? "guest";
            connectionString = $"amqp://{Uri.EscapeDataString(user)}:{Uri.EscapeDataString(pass)}@{host}:{port}{vhost}";
        }

        try
        {
            var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
            using var connection = factory.CreateConnectionAsync(cancellationToken).GetAwaiter().GetResult();
            return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ is reachable"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"RabbitMQ connectivity failed: {ex.Message}"));
        }
    }
}
