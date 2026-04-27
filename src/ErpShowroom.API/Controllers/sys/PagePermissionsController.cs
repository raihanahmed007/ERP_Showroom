using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PagePermissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PagePermissionsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Get all page permissions for a specific role</summary>
    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetByRoleId(long roleId)
    {
        var permissions = await _context.PagePermissions
            .Where(pp => pp.RoleId == roleId)
            .Select(pp => new
            {
                Id = pp.Id,
                RoleId = pp.RoleId,
                PageId = pp.PageId,
                PageName = pp.Page.PageName,
                PageRoute = pp.Page.RouteUrl,
                ModuleName = pp.Page.Feature.Module.ModuleName,
                FeatureName = pp.Page.Feature.FeatureName,
                CanView = pp.CanView,
                CanCreate = pp.CanCreate,
                CanEdit = pp.CanEdit,
                CanDelete = pp.CanDelete,
                CanApprove = pp.CanApprove,
                CanExport = pp.CanExport,
                CompanyId = pp.CompanyId
            })
            .ToListAsync();

        return Ok(permissions);
    }

    /// <summary>Bulk save page permissions for a role</summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkSave([FromBody] BulkSavePagePermissionsDto dto)
    {
        var existing = await _context.PagePermissions
            .Where(pp => pp.RoleId == dto.RoleId)
            .ToListAsync();

        _context.PagePermissions.RemoveRange(existing);

        foreach (var item in dto.Permissions)
        {
            var perm = new PagePermission
            {
                RoleId = dto.RoleId,
                PageId = item.PageId,
                CompanyId = item.CompanyId,
                CanView = item.CanView,
                CanCreate = item.CanCreate,
                CanEdit = item.CanEdit,
                CanDelete = item.CanDelete,
                CanApprove = item.CanApprove,
                CanExport = item.CanExport,
                IsActive = true
            };
            _context.PagePermissions.Add(perm);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = $"Saved {dto.Permissions.Count} page permissions for role." });
    }

    /// <summary>Save or update a single page permission for a role</summary>
    [HttpPost]
    public async Task<IActionResult> SavePermission([FromBody] SavePagePermissionDto dto)
    {
        var existing = await _context.PagePermissions
            .FirstOrDefaultAsync(pp => pp.RoleId == dto.RoleId && pp.PageId == dto.PageId && pp.CompanyId == dto.CompanyId);

        if (existing != null)
        {
            existing.CanView = dto.CanView;
            existing.CanCreate = dto.CanCreate;
            existing.CanEdit = dto.CanEdit;
            existing.CanDelete = dto.CanDelete;
            existing.CanApprove = dto.CanApprove;
            existing.CanExport = dto.CanExport;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var perm = new PagePermission
            {
                RoleId = dto.RoleId,
                PageId = dto.PageId,
                CompanyId = dto.CompanyId,
                CanView = dto.CanView,
                CanCreate = dto.CanCreate,
                CanEdit = dto.CanEdit,
                CanDelete = dto.CanDelete,
                CanApprove = dto.CanApprove,
                CanExport = dto.CanExport,
                IsActive = true
            };
            _context.PagePermissions.Add(perm);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Permission saved." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var perm = await _context.PagePermissions.FindAsync(id);
        if (perm == null) return NotFound();

        _context.PagePermissions.Remove(perm);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class SavePagePermissionDto
{
    public long RoleId { get; set; }
    public long PageId { get; set; }
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
}

public class BulkSavePagePermissionsDto
{
    public long RoleId { get; set; }
    public List<SavePagePermissionDto> Permissions { get; set; } = new();
}
