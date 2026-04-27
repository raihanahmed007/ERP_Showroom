using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.Common;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public UsersController(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Where(u => u.IsDeleted != true)
            .Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.Phone,
                EmployeeId = u.EmployeeId,
                IsActive = u.IsActive,
                LockoutEnd = u.LockoutEnd,
                FailedLoginCount = u.FailedLoginCount,
                CreatedAt = u.CreatedAt,
                Roles = u.Roles.Select(r => new { 
                    Id = r.Id, 
                    RoleId = r.RoleId, 
                    RoleName = r.Role.RoleName 
                }).ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _context.Users
            .Where(u => u.Id == id && u.IsDeleted != true)
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.NormalizedUserName,
                u.Email,
                u.Phone,
                u.EmployeeId,
                u.IsActive,
                u.LockoutEnd,
                u.FailedLoginCount,
                u.EmailConfirmed,
                u.PhoneConfirmed,
                u.TwoFactorEnabled,
                u.CreatedAt,
                Roles = u.Roles.Select(r => new { 
                    Id = r.Id, 
                    RoleId = r.RoleId, 
                    RoleName = r.Role.RoleName 
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Username and password are required.");

        // Check duplicate username
        var exists = await _context.Users.AnyAsync(u => u.UserName == dto.UserName && u.IsDeleted != true);
        if (exists) return BadRequest("Username already exists.");

        var user = new User
        {
            UserName = dto.UserName,
            NormalizedUserName = dto.UserName.ToUpperInvariant(),
            Email = dto.Email,
            Phone = dto.Phone,
            EmployeeId = dto.EmployeeId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 12),
            SecurityStamp = Guid.NewGuid().ToString(),
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { user.Id, user.UserName, user.Email });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || user.IsDeleted == true) return NotFound();

        // Check duplicate username (excluding self)
        if (!string.IsNullOrWhiteSpace(dto.UserName))
        {
            var exists = await _context.Users.AnyAsync(u => u.UserName == dto.UserName && u.Id != id && u.IsDeleted != true);
            if (exists) return BadRequest("Username already exists.");
            user.UserName = dto.UserName;
            user.NormalizedUserName = dto.UserName.ToUpperInvariant();
        }

        user.Email = dto.Email ?? user.Email;
        user.Phone = dto.Phone ?? user.Phone;
        user.EmployeeId = dto.EmployeeId ?? user.EmployeeId;
        user.IsActive = dto.IsActive ?? user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(long id, [FromBody] ResetPasswordDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || user.IsDeleted == true) return NotFound();

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword, 12);
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.FailedLoginCount = 0;
        user.LockoutEnd = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Password reset successfully." });
    }

    [HttpPut("{id}/unlock")]
    public async Task<IActionResult> UnlockUser(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || user.IsDeleted == true) return NotFound();

        user.FailedLoginCount = 0;
        user.LockoutEnd = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Account unlocked." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        // Soft delete
        user.IsDeleted = true;
        user.IsActive = false;
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}

// DTOs for proper user management
public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? EmployeeId { get; set; }
}

public class UpdateUserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? EmployeeId { get; set; }
    public bool? IsActive { get; set; }
}

public class ResetPasswordDto
{
    public string NewPassword { get; set; } = string.Empty;
}
