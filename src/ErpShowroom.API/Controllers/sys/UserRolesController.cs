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
public class UserRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserRolesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Get all role assignments for a specific user</summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => new
            {
                ur.Id,
                ur.UserId,
                ur.RoleId,
                RoleName = ur.Role != null ? ur.Role.RoleName : "",
                RoleType = ur.Role != null ? ur.Role.RoleType.ToString() : "",
                ur.IsActive
            })
            .ToListAsync();

        return Ok(roles);
    }

    /// <summary>Get all available roles</summary>
    [HttpGet("available-roles")]
    public async Task<IActionResult> GetAvailableRoles()
    {
        var roles = await _context.Set<Role>()
            .Where(r => r.IsActive == true && r.IsDeleted != true)
            .Select(r => new { r.Id, r.RoleName, r.RoleCode, RoleType = r.RoleType.ToString(), r.Description })
            .ToListAsync();

        return Ok(roles);
    }

    /// <summary>Assign a role to a user</summary>
    [HttpPost]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
    {
        // Check if already assigned
        var exists = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);

        if (exists)
            return BadRequest("Role is already assigned to this user.");

        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            IsActive = true
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        return Ok(new { userRole.Id, message = "Role assigned successfully." });
    }

    /// <summary>Remove a role assignment</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveRole(long id)
    {
        var userRole = await _context.UserRoles.FindAsync(id);
        if (userRole == null) return NotFound();

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class AssignRoleDto
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}
