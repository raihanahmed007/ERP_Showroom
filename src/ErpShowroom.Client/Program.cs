// ErpShowroom.Client/Program.cs
using Blazored.LocalStorage;
using ErpShowroom.Client.Services;
using ErpShowroom.Client.Services.acc;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<ErpShowroom.Client.App>("#app");

// HttpClient pointed at the API
Console.WriteLine($"Runtime Origin: {builder.HostEnvironment.BaseAddress}");
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// MudBlazor
builder.Services.AddMudServices();

// Theme Service
builder.Services.AddSingleton<ThemeService>();

// LocalStorage (Blazored)
builder.Services.AddBlazoredLocalStorage();

// Auth infrastructure — order matters:
// 1. Register CustomAuthStateProvider as itself first
builder.Services.AddScoped<CustomAuthStateProvider>();
// 2. Also expose it as the framework's AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Auth options
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// ApiClient — registered last because it depends on CustomAuthStateProvider
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IAccountingService, AccountingService>();
builder.Services.AddScoped<ISystemService, SystemService>();

await builder.Build().RunAsync();