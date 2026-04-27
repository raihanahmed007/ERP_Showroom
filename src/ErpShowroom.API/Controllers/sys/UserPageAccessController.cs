using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserPageAccessController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserPageAccessController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Get all page access overrides for a specific user</summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        var accesses = await _context.Set<UserPageAccess>()
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                Id = x.Id,
                UserId = x.UserId,
                PageId = x.PageId,
                PageName = x.Page.PageName,
                PageRoute = x.Page.RouteUrl,
                ModuleName = x.Page.Feature.Module.ModuleName,
                FeatureName = x.Page.Feature.FeatureName,
                CanView = x.CanView,
                CanCreate = x.CanCreate,
                CanEdit = x.CanEdit,
                CanDelete = x.CanDelete,
                CanApprove = x.CanApprove,
                CanExport = x.CanExport,
                OverridesRole = x.OverridesRole,
                CompanyId = x.CompanyId
            })
            .ToListAsync();

        return Ok(accesses);
    }

    /// <summary>Get all available pages for dropdown</summary>
    [HttpGet("available-pages")]
    public async Task<IActionResult> GetAvailablePages()
    {
        var pages = await _context.Set<AppPage>()
            .Where(p => p.IsActive == true && p.IsDeleted != true)
            .Select(p => new
            {
                Id = p.Id,
                PageName = p.PageName,
                PageCode = p.PageCode,
                RouteUrl = p.RouteUrl,
                ModuleName = p.Feature.Module.ModuleName,
                FeatureName = p.Feature.FeatureName,
                PageType = p.PageType != null ? p.PageType.ToString() : null
            })
            .OrderBy(p => p.ModuleName)
            .ThenBy(p => p.PageName)
            .ToListAsync();

        return Ok(pages);
    }

    /// <summary>Save or update page access for a user (upsert)</summary>
    [HttpPost]
    public async Task<IActionResult> SaveAccess([FromBody] SaveUserPageAccessDto dto)
    {
        var existing = await _context.Set<UserPageAccess>()
            .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.PageId == dto.PageId && x.CompanyId == dto.CompanyId);

        if (existing != null)
        {
            existing.CanView = dto.CanView;
            existing.CanCreate = dto.CanCreate;
            existing.CanEdit = dto.CanEdit;
            existing.CanDelete = dto.CanDelete;
            existing.CanApprove = dto.CanApprove;
            existing.CanExport = dto.CanExport;
            existing.OverridesRole = dto.OverridesRole;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var access = new UserPageAccess
            {
                UserId = dto.UserId,
                PageId = dto.PageId,
                CompanyId = dto.CompanyId,
                CanView = dto.CanView,
                CanCreate = dto.CanCreate,
                CanEdit = dto.CanEdit,
                CanDelete = dto.CanDelete,
                CanApprove = dto.CanApprove,
                CanExport = dto.CanExport,
                OverridesRole = dto.OverridesRole,
                IsActive = true
            };
            _context.Set<UserPageAccess>().Add(access);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Page access saved." });
    }

    /// <summary>Bulk save page access for a user</summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkSave([FromBody] BulkSaveUserPageAccessDto dto)
    {
        // Remove existing overrides for this user
        var existing = await _context.Set<UserPageAccess>()
            .Where(x => x.UserId == dto.UserId)
            .ToListAsync();

        _context.Set<UserPageAccess>().RemoveRange(existing);

        // Add new ones
        foreach (var item in dto.Accesses)
        {
            var access = new UserPageAccess
            {
                UserId = dto.UserId,
                PageId = item.PageId,
                CompanyId = item.CompanyId,
                CanView = item.CanView,
                CanCreate = item.CanCreate,
                CanEdit = item.CanEdit,
                CanDelete = item.CanDelete,
                CanApprove = item.CanApprove,
                CanExport = item.CanExport,
                OverridesRole = item.OverridesRole,
                IsActive = true
            };
            _context.Set<UserPageAccess>().Add(access);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = $"Saved {dto.Accesses.Count} page access overrides." });
    }

    /// <summary>Delete a specific page access override</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var access = await _context.Set<UserPageAccess>().FindAsync(id);
        if (access == null) return NotFound();

        _context.Set<UserPageAccess>().Remove(access);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class SaveUserPageAccessDto
{
    public long UserId { get; set; }
    public long PageId { get; set; }
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
    public bool OverridesRole { get; set; }
}

public class BulkSaveUserPageAccessDto
{
    public long UserId { get; set; }
    public List<SaveUserPageAccessDto> Accesses { get; set; } = new();
}
