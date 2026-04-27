using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _context.Set<Role>()
            .Where(r => r.IsDeleted != true)
            .Select(r => new
            {
                r.Id,
                r.RoleName,
                r.RoleCode,
                r.Description,
                RoleType = r.RoleType.ToString(),
                r.IsActive,
                r.CreatedAt
            })
            .ToListAsync();

        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var role = await _context.Set<Role>()
            .Where(r => r.Id == id && r.IsDeleted != true)
            .Select(r => new
            {
                r.Id,
                r.RoleName,
                r.RoleCode,
                r.Description,
                RoleType = r.RoleType.ToString(),
                r.IsActive
            })
            .FirstOrDefaultAsync();

        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateUpdateDto dto)
    {
        var exists = await _context.Set<Role>()
            .AnyAsync(r => r.RoleCode == dto.RoleCode && r.IsDeleted != true);
        if (exists) return BadRequest("Role code already exists.");

        var role = new Role
        {
            RoleName = dto.RoleName,
            RoleCode = dto.RoleCode,
            Description = dto.Description,
            RoleType = (RoleType)(dto.RoleType ?? 2),
            IsActive = true
        };

        _context.Set<Role>().Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, new { role.Id, role.RoleName });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] RoleCreateUpdateDto dto)
    {
        var role = await _context.Set<Role>().FindAsync(id);
        if (role == null || role.IsDeleted == true) return NotFound();

        // Check duplicate code (excluding self)
        if (!string.IsNullOrWhiteSpace(dto.RoleCode))
        {
            var exists = await _context.Set<Role>()
                .AnyAsync(r => r.RoleCode == dto.RoleCode && r.Id != id && r.IsDeleted != true);
            if (exists) return BadRequest("Role code already exists.");
            role.RoleCode = dto.RoleCode;
        }

        role.RoleName = dto.RoleName ?? role.RoleName;
        role.Description = dto.Description ?? role.Description;
        if (dto.RoleType.HasValue) role.RoleType = (RoleType)dto.RoleType.Value;
        role.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var role = await _context.Set<Role>().FindAsync(id);
        if (role == null) return NotFound();

        role.IsDeleted = true;
        role.IsActive = false;
        role.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class RoleCreateUpdateDto
{
    public string RoleName { get; set; } = string.Empty;
    public string? RoleCode { get; set; }
    public string? Description { get; set; }
    public int? RoleType { get; set; }
}
