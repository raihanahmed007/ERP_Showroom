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
            // until the transaction commits. (For true persistence, AddEntityFrameworkOutbox is recommended).
            x.AddInMemoryOutbox();

            // Setup Mediator strictly for internal domain handling if requested for development
            // But per prompt explicitly handling RabbitMQ "for production use AddBus".
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Configure retry policy: retry 3 times with exponential backoff (1s, 2s, 4s).
                // Delta is 1s, so 1s, 2s, 4s aligns exactly with intervals of exponentially backed intervals.
                cfg.UseMessageRetry(r => r.Exponential(
                    retryLimit: 3, 
                    minInterval: TimeSpan.FromSeconds(1), 
                    maxInterval: TimeSpan.FromSeconds(4), 
                    intervalDelta: TimeSpan.FromSeconds(1)));

                // Dead-Letter Handling: MassTransit inherently routes failed messages (post-retries) 
                // to a '{queue_name}_error' queue explicitly. Our error queues naturally exist natively inside RMQ.
                
                // Configures endpoints mapped implicitly using standard KebabCase patterns (e.g. 'sms' -> 'sms_error')
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
