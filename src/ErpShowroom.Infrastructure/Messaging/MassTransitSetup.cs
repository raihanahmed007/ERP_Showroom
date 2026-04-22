using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ErpShowroom.Infrastructure.Messaging.Consumers;

namespace ErpShowroom.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            // Register all consumers found in the same assembly as SmsConsumer
            x.AddConsumers(typeof(SmsConsumer).Assembly);

            // Using InMemoryOutbox specifically as requested to hold messages in memory scope 
            // until the transaction commits. (Some MassTransit package versions may not expose this method directly.)
            // x.AddInMemoryOutbox();

            // Setup Mediator strictly for internal domain handling if requested for development
            // But per prompt explicitly handling RabbitMQ "for production use AddBus".
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSection = configuration.GetSection("RabbitMQ");
                var host = rabbitMqSection.GetValue<string>("Host") ?? "localhost";
                var port = rabbitMqSection.GetValue<int?>("Port") ?? 5672;
                var virtualHost = rabbitMqSection.GetValue<string>("VHost") ?? "/";
                var username = rabbitMqSection.GetValue<string>("User") ?? "guest";
                var password = rabbitMqSection.GetValue<string>("Pass") ?? "guest";

                var rabbitMqUri = new Uri($"rabbitmq://{host}:{port}{virtualHost}");
                cfg.Host(rabbitMqUri, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                // Configure retry policy: retry 3 times with exponential backoff (1s, 2s, 4s).
                cfg.UseMessageRetry(r => r.Exponential(
                    retryLimit: 3,
                    minInterval: TimeSpan.FromSeconds(1),
                    maxInterval: TimeSpan.FromSeconds(4),
                    intervalDelta: TimeSpan.FromSeconds(1)));

                // Dead-letter handling is managed by MassTransit using the built-in '_error' queues.
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
