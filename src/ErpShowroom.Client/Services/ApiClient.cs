using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ErpShowroom.Application.sys.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ErpShowroom.Client.Services;

public interface IApiClient
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    Task LogoutAsync();
    Task<T?> GetAsync<T>(string url);
    Task<HttpResponseMessage> PostAsync<T>(string url, T data);
    Task<HttpResponseMessage> PostContentAsync(string url, HttpContent content);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    public ApiClient(HttpClient httpClient, ILocalStorageService localStorage, 
                     AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        _navigationManager = navigationManager;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", request);
        if (response.IsSuccessStatusCode)
        {
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (authResult != null)
            {
                await _localStorage.SetItemAsync("authToken", authResult.AccessToken);
                await _localStorage.SetItemAsync("refreshToken", authResult.RefreshToken);
                await _authStateProvider.NotifyUserAuthentication(authResult.AccessToken);
                return authResult;
            }
        }
        return null;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("auth/logout", null);
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        await _authStateProvider.NotifyUserLogout();
        _navigationManager.NavigateTo("/login");
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        await AddAuthHeader();
        var response = await _httpClient.GetAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (await RefreshTokenAsync())
            {
                await AddAuthHeader();
                response = await _httpClient.GetAsync(url);
            }
            else
            {
                await LogoutAsync();
                return default;
            }
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
    {
        await AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync(url, data);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (await RefreshTokenAsync())
            {
                await AddAuthHeader();
                response = await _httpClient.PostAsJsonAsync(url, data);
            }
            else
            {
                await LogoutAsync();
            }
        }
        return response;
    }

    public async Task<HttpResponseMessage> PostContentAsync(string url, HttpContent content)
    {
        await AddAuthHeader();
        var response = await _httpClient.PostAsync(url, content);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (await RefreshTokenAsync())
            {
                await AddAuthHeader();
                response = await _httpClient.PostAsync(url, content);
            }
            else
            {
                await LogoutAsync();
            }
        }
        return response;
    }

    private async Task AddAuthHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
        if (string.IsNullOrEmpty(refreshToken)) return false;

        var response = await _httpClient.PostAsJsonAsync("auth/refresh", new RefreshTokenRequestDto { RefreshToken = refreshToken });
        if (response.IsSuccessStatusCode)
        {
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (authResult != null)
            {
                await _localStorage.SetItemAsync("authToken", authResult.AccessToken);
                await _localStorage.SetItemAsync("refreshToken", authResult.RefreshToken);
                return true;
            }
        }
        return false;
    }
}
