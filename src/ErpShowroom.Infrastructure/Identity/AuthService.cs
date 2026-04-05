using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ErpShowroom.Application.sys.Interfaces;
using ErpShowroom.Infrastructure.Persistence;
using ErpShowroom.Domain.sys.Entities;

namespace ErpShowroom.Infrastructure.Identity;

public class AuthService(AppDbContext db, IConfiguration configuration) : IAuthService
{
    public async Task<AuthResult?> LoginAsync(LoginRequest request)
    {
        var user = await db.Users
            .Include(u => u.UserRoles!)
                .ThenInclude(ur => ur.Role!)
                    .ThenInclude(r => r.RolePermissions!)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Username || u.Phone == request.Username);
            
        if (user == null || user.IsActive == false) return null;
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResult?> RefreshAsync(RefreshRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) return null;

        var username = principal.Identity?.Name;
        var user = await db.Users
            .Include(u => u.UserRoles!).ThenInclude(ur => ur.Role!).ThenInclude(r => r.RolePermissions!).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) return null;

        return await GenerateTokensAsync(user);
    }

    public async Task<bool> LogoutAsync(string username)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await db.SaveChangesAsync();
        return true;
    }

    private async Task<AuthResult> GenerateTokensAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.UserRoles != null)
        {
            foreach (var ur in user.UserRoles)
            {
                if (ur.Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ur.Role.RoleName!));
                    if (ur.Role.RolePermissions != null)
                    {
                        foreach (var rp in ur.Role.RolePermissions)
                        {
                            if (rp.Permission != null) 
                                claims.Add(new Claim("Permission", rp.Permission.PermissionKey!));
                        }
                    }
                }
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await db.SaveChangesAsync();

        return new AuthResult(accessToken, refreshToken);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            ValidateLifetime = false 
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
