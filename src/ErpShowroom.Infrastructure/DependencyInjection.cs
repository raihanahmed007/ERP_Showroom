using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.Common;
using ErpShowroom.Infrastructure.Persistence;
using ErpShowroom.Infrastructure.BackgroundJobs;
using ErpShowroom.Infrastructure.Messaging;
using ErpShowroom.Infrastructure.AI;
using ErpShowroom.Infrastructure.OCR;

namespace ErpShowroom.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ErpShowroom.Domain.Common.IUnitOfWork, UnitOfWork>();
        services.AddScoped<ErpShowroom.Application.sys.Interfaces.IAuthService, ErpShowroom.Infrastructure.Identity.AuthService>();

        services.AddRedisCache(configuration);
        services.AddAndUseHangfire(configuration);
        services.AddMassTransitWithRabbitMq(configuration);
        services.AddOllamaServices(configuration);
        services.AddTesseractOcr(configuration);
        services.AddScoped<ErpShowroom.Infrastructure.Persistence.Services.QueryOptimizationService>();

        return services;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        var options = StackExchange.Redis.ConfigurationOptions.Parse(redisConn);
        options.AbortOnConnectFail = false;

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp => 
            StackExchange.Redis.ConnectionMultiplexer.Connect(options));
            
        services.AddSingleton<ErpShowroom.Application.Common.Interfaces.ICacheService, ErpShowroom.Infrastructure.Caching.RedisCacheService>();
        
        return services;
    }
}

//# Set default project to ErpShowroom.Infrastructure
//Set-DefaultProject ErpShowroom.Infrastructure

//# Create migration
//Add-Migration InitialCreate -StartupProject ErpShowroom.API

//# Apply to database
//Update-Database -StartupProject ErpShowroom.API
