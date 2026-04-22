using Blazored.LocalStorage;
using ErpShowroom.Application.sys.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ErpShowroom.Client.Services;

public interface IApiClient
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    Task LogoutAsync();
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object body);
    Task PostAsync(string endpoint, object? body = null);
    Task<bool> PostAsyncBool(string endpoint, object body);
    Task<HttpResponseMessage> PostContentAsync(string endpoint, HttpContent content);
    Task<bool> PutAsync(string endpoint, object body);
    Task<bool> DeleteAsync(string endpoint);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authProvider;
    private readonly NavigationManager _nav;

    public ApiClient(
        HttpClient http,
        ILocalStorageService localStorage,
        CustomAuthStateProvider authProvider,
        NavigationManager nav)
    {
        _http = http;
        _localStorage = localStorage;
        _authProvider = authProvider;
        _nav = nav;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Login Failed: {response.StatusCode} - {error}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (result == null) return null;

            await _authProvider.NotifyUserLoggedIn(result.AccessToken);
            await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);
            SetBearerToken(result.AccessToken);

            return result;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"API Login Exception: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Login General Error: {ex.Message}");
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await AttachTokenAsync();
            await _http.PostAsync("auth/logout", null);
        }
        catch { }
        finally
        {
            // ✅ NotifyUserLoggedOut clears cache + removes tokens from localStorage
            await _authProvider.NotifyUserLoggedOut();
            _http.DefaultRequestHeaders.Authorization = null;
            _nav.NavigateTo("/login", forceLoad: false); // ✅ SPA nav
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await AttachTokenAsync();
        var response = await _http.GetAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return default;
            response = await _http.GetAsync(endpoint);
        }

        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T?> PostAsync<T>(string endpoint, object body)
    {
        await AttachTokenAsync();
        var response = await _http.PostAsJsonAsync(endpoint, body);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return default;
            response = await _http.PostAsJsonAsync(endpoint, body);
        }

        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task PostAsync(string endpoint, object? body = null)
    {
        await AttachTokenAsync();

        var response = body is null
            ? await _http.PostAsync(endpoint, null)
            : await _http.PostAsJsonAsync(endpoint, body);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return;
            _ = body is null
                ? await _http.PostAsync(endpoint, null)
                : await _http.PostAsJsonAsync(endpoint, body);
        }
    }

    public async Task<bool> PutAsync(string endpoint, object body)
    {
        await AttachTokenAsync();
        var response = await _http.PutAsJsonAsync(endpoint, body);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return false;
            response = await _http.PutAsJsonAsync(endpoint, body);
        }

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        await AttachTokenAsync();
        var response = await _http.DeleteAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return false;
            response = await _http.DeleteAsync(endpoint);
        }

        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> PostContentAsync(string endpoint, HttpContent content)
    {
        await AttachTokenAsync();
        var response = await _http.PostAsync(endpoint, content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return response;
            response = await _http.PostAsync(endpoint, content);
        }

        return response;
    }

    public async Task<bool> PostAsyncBool(string endpoint, object body)
    {
        await AttachTokenAsync();
        var response = await _http.PostAsJsonAsync(endpoint, body);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (!await TryRefreshTokenAsync()) return false;
            response = await _http.PostAsJsonAsync(endpoint, body);
        }

        return response.IsSuccessStatusCode;
    }

    private async Task AttachTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("access_token");
        if (!string.IsNullOrWhiteSpace(token))
            SetBearerToken(token);
    }

    private void SetBearerToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<bool> TryRefreshTokenAsync()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>("refresh_token");
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            await ForcedLogoutAsync();
            return false;
        }

        var response = await _http.PostAsJsonAsync("auth/refresh",
            new RefreshTokenRequestDto { RefreshToken = refreshToken });

        if (!response.IsSuccessStatusCode)
        {
            await ForcedLogoutAsync();
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (result == null)
        {
            await ForcedLogoutAsync();
            return false;
        }

        await _authProvider.NotifyUserLoggedIn(result.AccessToken); // ✅ awaited
        await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);
        SetBearerToken(result.AccessToken);

        return true;
    }

    private async Task ForcedLogoutAsync()
    {
        await _authProvider.NotifyUserLoggedOut(); // ✅ clears cache + localStorage
        _http.DefaultRequestHeaders.Authorization = null;
        _nav.NavigateTo("/login", forceLoad: false); // ✅ SPA nav
    }
}