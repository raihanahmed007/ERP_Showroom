using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace ErpShowroom.API.HealthChecks;

public class TesseractHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public TesseractHealthCheck(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var tessDataPath = _configuration["Tesseract:TessDataPath"] ?? "./tessdata";
        
        // Resolve path based on environment
        var absolutePath = Path.IsPathRooted(tessDataPath) 
            ? tessDataPath 
            : Path.Combine(_environment.ContentRootPath, tessDataPath);

        if (Directory.Exists(absolutePath))
        {
            var engFile = Path.Combine(absolutePath, "eng.traineddata");
            var benFile = Path.Combine(absolutePath, "ben.traineddata");

            if (File.Exists(engFile) && File.Exists(benFile))
            {
                return Task.FromResult(HealthCheckResult.Healthy($"Tesseract data found at {absolutePath} (eng, ben)"));
            }

            return Task.FromResult(HealthCheckResult.Degraded($"TessDataPath exists but missing traineddata files in {absolutePath}"));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy($"TessDataPath not found at {absolutePath}"));
    }
}
