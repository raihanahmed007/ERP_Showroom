using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ErpShowroom.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private AuthenticationState? _cachedState; // ✅ in-memory cache — instant on navigation

    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // ✅ Return cached state instantly — no async wait — fixes redirect race condition
        if (_cachedState != null)
            return _cachedState;

        try
        {
            var token = await _localStorage.GetItemAsync<string>("access_token");

            if (string.IsNullOrWhiteSpace(token))
                return Anonymous;

            if (IsTokenExpired(token))
            {
                await _localStorage.RemoveItemAsync("access_token");
                return Anonymous;
            }

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(identity)); // ✅ cache it
            return _cachedState;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"⚠️ GetAuthenticationStateAsync failed: {ex.Message}");
            return Anonymous;
        }
    }

    public async Task NotifyUserLoggedIn(string accessToken)
    {
        try
        {
            // ✅ Save to localStorage first
            await _localStorage.SetItemAsync("access_token", accessToken);

            var claims = ParseClaimsFromJwt(accessToken);
            var identity = new ClaimsIdentity(claims, authenticationType: "jwt");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(identity)); // ✅ cache immediately
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Auth parse error: {ex.Message}");
            await NotifyUserLoggedOut();
        }
    }

    public async Task NotifyUserLoggedOut()
    {
        _cachedState = null; // ✅ clear cache
        await _localStorage.RemoveItemAsync("access_token");
        await _localStorage.RemoveItemAsync("refresh_token");
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    private static bool IsTokenExpired(string jwt)
    {
        try
        {
            var payload = GetJwtPayload(jwt);
            if (payload.TryGetValue("exp", out var expValue))
            {
                var exp = expValue.GetInt64();
                var expiry = DateTimeOffset.FromUnixTimeSeconds(exp);
                return expiry < DateTimeOffset.UtcNow;
            }
            return false;
        }
        catch { return true; }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();

        try
        {
            var payload = GetJwtPayload(jwt);

            foreach (var kvp in payload)
            {
                var type = kvp.Key;
                var element = kvp.Value;

                try
                {
                    if (element.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in element.EnumerateArray())
                            claims.Add(new Claim(type, item.ToString()));
                    }
                    else if (type is "role" or "roles" or "permissions" or "permission")
                    {
                        var raw = element.ToString();
                        if (raw.StartsWith("[") && raw.EndsWith("]"))
                        {
                            var array = JsonSerializer.Deserialize<string[]>(raw);
                            if (array != null)
                                foreach (var part in array) claims.Add(new Claim(type, part));
                        }
                        else
                        {
                            foreach (var part in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                claims.Add(new Claim(type, part.Trim()));
                        }
                    }
                    else
                    {
                        // Map short names to standard claim types if needed
                        var claimType = type switch
                        {
                            "unique_name" => ClaimTypes.Name,
                            "nameid" => ClaimTypes.NameIdentifier,
                            "email" => ClaimTypes.Email,
                            "role" => ClaimTypes.Role,
                            _ => type
                        };
                        claims.Add(new Claim(claimType, element.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Failed to parse claim '{type}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ JWT parsing failed: {ex.Message}");
            throw;
        }

        return claims;
    }

    private static Dictionary<string, JsonElement> GetJwtPayload(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
            throw new FormatException("Invalid JWT format.");

        var payloadBase64 = parts[1]
            .Replace('-', '+')
            .Replace('_', '/');

        switch (payloadBase64.Length % 4)
        {
            case 2: payloadBase64 += "=="; break;
            case 3: payloadBase64 += "="; break;
        }

        var payloadBytes = Convert.FromBase64String(payloadBase64);
        var payloadJson = Encoding.UTF8.GetString(payloadBytes);

        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadJson)
               ?? new Dictionary<string, JsonElement>();
    }
}