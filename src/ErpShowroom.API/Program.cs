using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using ErpShowroom.Application;
using ErpShowroom.Infrastructure;
using RabbitMQ.Client;
using ErpShowroom.Infrastructure.BackgroundJobs;
using ErpShowroom.Infrastructure.OCR;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IO;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ErpShowroom.API.HealthChecks;
using ErpShowroom.API.Middleware;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Infrastructure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Enable Static Web Assets for development (Required for Blazor WASM Hosted)
builder.WebHost.UseStaticWebAssets();

// -- 1. Serilog Logging ----------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File("logs/erp-showroom-.log", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// // -- 2. OpenTelemetry (Tracing + Metrics) ----------------------------------
// builder.Services.AddOpenTelemetry()
//     .WithTracing(tracing => tracing
//         .SetResourceBuilder(ResourceBuilder.CreateDefault()
//             .AddService("ErpShowroom.API", serviceVersion: "1.0.0"))
//         .AddAspNetCoreInstrumentation(opt => 
//         {
//             opt.RecordException = true;
//         })
//         .AddEntityFrameworkCoreInstrumentation(opt =>
//         {
//             opt.SetDbStatementForText = true;
//             opt.SetDbStatementForStoredProcedure = true;
//         })
//         .AddHttpClientInstrumentation()
//         .AddOtlpExporter(opt => 
//         {
//             // Jaeger supports OTLP on port 4317 (gRPC) or 4318 (HTTP/JSON)
//             // User requested http://localhost:14268/api/traces (HTTP Thrift)
//             // Note: Modern OTLP exporter is more robust. 
//             opt.Endpoint = new Uri("http://localhost:4317"); 
//         }))
//     .WithMetrics(metrics => metrics
//         .AddAspNetCoreInstrumentation()
//         .AddMeter("ErpShowroom.Metrics"));

// -- 3. Application + Infrastructure -----------------------------------------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEncryptionService, DataProtectionEncryptionService>();
builder.Services.AddScoped<ErpShowroom.Application.fin.Workflows.WorkflowOrchestrator>();

// -- Data Protection --
var dpBuilder = builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ErpShowroom-Keys")));

if (OperatingSystem.IsWindows())
{
    dpBuilder = dpBuilder.ProtectKeysWithDpapi();
}

dpBuilder.SetApplicationName("ErpShowroom");

// -- 4. Register Services --------------------------------------------------
builder.Services.AddTesseractOcr(builder.Configuration);

// -- 5. JWT Auth -----------------------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Removed FallbackPolicy to allow anonymous access to SPA routes
});
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, ErpShowroom.API.Authorization.PermissionPolicyProvider>();
builder.Services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, ErpShowroom.API.Authorization.PermissionAuthorizationHandler>();

// builder.Services.AddRateLimiter(options =>
// {
//     options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

//     options.AddFixedWindowLimiter("ApiLimiter", opt =>
//     {
//         opt.Window = TimeSpan.FromMinutes(1);
//         opt.PermitLimit = 100;
//         opt.QueueLimit = 0;
//     });

//     options.AddFixedWindowLimiter("LoginLimiter", opt =>
//     {
//         opt.Window = TimeSpan.FromMinutes(1);
//         opt.PermitLimit = 5;
//         opt.QueueLimit = 0;
//     });
// });

builder.Services.AddHttpClient("Ollama", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AI:OllamaBaseUrl"] ?? "http://localhost:11434");
    client.Timeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int?>("AI:TimeoutSeconds") ?? 30);
});

// -- 6. OpenAPI (minimal setup) -----------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// -- 7. Health Checks ------------------------------------------------------
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "sqlserver")
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!, name: "redis")
    .AddCheck<RabbitMqHealthCheck>("rabbitmq")
    .AddCheck<TesseractHealthCheck>("tesseract")
    .AddCheck<OllamaHealthCheck>("ollama");

// -- 8. CORS ---------------------------------------------------------------
builder.Services.AddCors(opt => 
{
    opt.AddPolicy("ErpCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

// app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseCors("ErpCorsPolicy");

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Detailed Health Check Response
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ---- Database Initialization ----
using (var scope = app.Services.CreateScope())
{
    try 
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ErpShowroom.Infrastructure.Persistence.AppDbContext>();
        dbContext.Database.Migrate();
        await ErpShowroom.Infrastructure.Persistence.DatabaseSeeder.SeedAsync(dbContext);
        Log.Information("Database initialization successful");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred during database initialization");
    }
}

app.Run();

public partial class Program { }
