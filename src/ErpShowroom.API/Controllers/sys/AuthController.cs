using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;
using ErpShowroom.Infrastructure.Persistence;
using ErpShowroom.Application.sys.DTOs;
using ErpShowroom.Domain.sys.Entities;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.Roles!)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == request.UsernamePhoneOrEmail || u.Email == request.UsernamePhoneOrEmail || u.Phone == request.UsernamePhoneOrEmail);

        if (user == null)
            return Unauthorized("Invalid credentials.");

        if (user.IsActive != true)
            return Unauthorized("Account is disabled.");

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Unauthorized("Account is locked. Try again later.");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
            return await HandleFailedLogin(user);

        // Success - Reset Failed Login Tracking
        user.FailedLoginCount = 0;
        user.LockoutEnd = null;

        return await GenerateAndReturnTokens(user);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.Roles!)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow || user.IsActive != true)
            return Unauthorized("Invalid or expired refresh token.");

        // Rotate token
        return await GenerateAndReturnTokens(user);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (long.TryParse(userIdString, out var userId))
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _context.SaveChangesAsync();
            }
        }
        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdString, out var userId))
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.IsActive != true)
            return Unauthorized("User not found or inactive.");

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            return BadRequest("Incorrect old password.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, 12);
        
        // Invalidate current refresh tokens so other sessions are forced out next refresh
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully" });
    }

    private async Task<IActionResult> HandleFailedLogin(User user)
    {
        user.FailedLoginCount = (user.FailedLoginCount ?? 0) + 1;

        if (user.FailedLoginCount >= 5)
        {
            user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();
            return Unauthorized("Account is locked. Try again in 15 minutes.");
        }

        await _context.SaveChangesAsync();
        return Unauthorized("Invalid credentials.");
    }

    private async Task<IActionResult> GenerateAndReturnTokens(User user)
    {
        var permissions = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id && ur.Role != null && ur.Role.IsActive == true)
            .SelectMany(ur => _context.RolePermissions.Where(rp => rp.RoleId == ur.RoleId))
            .Select(rp => rp.Permission!.PermissionKey!)
            .Distinct()
            .ToListAsync();

        var roles = user.Roles?
            .Select(r => r.Role?.RoleName)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n!)
            .ToList() ?? new List<string>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("roles", string.Join(",", roles))
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role!));
        }

        foreach (var p in permissions)
        {
            claims.Add(new Claim("permissions", p));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "default_secret_key_needs_to_be_long_enough"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expiry = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        string refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 days as requested

        await _context.SaveChangesAsync();

        return Ok(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty
        });
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
