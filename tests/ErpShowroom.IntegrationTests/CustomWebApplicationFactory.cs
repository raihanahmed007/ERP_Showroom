using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ErpShowroom.Infrastructure.Persistence;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ErpShowroom.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _sqlConnectionString;
    private readonly string _redisConnectionString;
    private readonly string _rabbitmqConnectionString;

    public CustomWebApplicationFactory(ContainerFixture fixture)
    {
        _sqlConnectionString = fixture.SqlContainer.GetConnectionString();
        _redisConnectionString = fixture.RedisContainer.GetConnectionString();
        _rabbitmqConnectionString = fixture.RabbitMqContainer.GetConnectionString();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove real DbContext
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add Testcontainers DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(_sqlConnectionString);
            });

            // Override Caching / Redis if configured via connection strings
            // (Assuming configuration is used in Infrastructure registration)
            // You can also mock other services here.

            // Authentication bypass
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });
    }

    public async Task SeedDataAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        // Seed minimal data for tests
        if (!context.Customers.Any())
        {
            context.Customers.Add(new ErpShowroom.Domain.crm.Entities.Customer { Id = 1, CustomerName = "Test Customer", Phone = "123456" });
            context.Products.Add(new ErpShowroom.Domain.inv.Entities.Product { Id = 1, ProductCode = "P001", ProductName = "Test Product", UnitPrice = 100000 });
            await context.SaveChangesAsync();
        }
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { 
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "FinanceManager"),
            new Claim("sub", "1")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
