using System;
using System.IO;
using ErpShowroom.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ErpShowroom.Infrastructure.OCR;

public static class OcrRegistration
{
    public static IServiceCollection AddTesseractOcr(this IServiceCollection services, IConfiguration configuration)
    {
        // Register OcrService as singleton (lazy loading handles internal Tesseract instantiation)
        services.AddSingleton<ITesseractOcrService, TesseractOcrService>();

        // Check if tessdata exists at startup and warn if not
        var tessDataPath = configuration.GetValue<string>("Tesseract:TessDataPath") ?? "./tessdata";
        
        // This log check usually requires an ILogger, but we can't easily get it here at registration.
        // However, we can perform directory check.
        if (!Directory.Exists(tessDataPath))
        {
            Console.WriteLine($"[WARNING] Tesseract tessdata directory not found at: {tessDataPath}. " +
                              "Please ensure .traineddata files (eng, ben) are present there.");
        }

        return services;
    }
}
