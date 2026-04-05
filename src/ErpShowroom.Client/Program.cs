using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ErpShowroom.Client.Services;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<ErpShowroom.Client.App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") }); // Replace with actual API URL
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IApiClient, ApiClient>();

await builder.Build().RunAsync();
