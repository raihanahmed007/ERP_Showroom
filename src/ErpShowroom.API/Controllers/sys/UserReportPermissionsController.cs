using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.API.Controllers.sys;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserReportPermissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserReportPermissionsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Get all report permissions for a specific user</summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        var permissions = await _context.UserReportPermissions
            .Where(p => p.UserId == userId)
            .Include(p => p.ReportName)
                .ThenInclude(r => r.ReportType)
                    .ThenInclude(rt => rt.Module)
            .Select(p => new
            {
                p.Id,
                p.UserId,
                p.ReportNameId,
                ReportNameText = p.ReportName.Name,
                ReportCode = p.ReportName.ReportCode,
                ReportTypeName = p.ReportName.ReportType.ReportTypeName,
                ModuleName = p.ReportName.ReportType.Module.ModuleName,
                p.CanView,
                p.CanExport,
                p.CanPrint,
                p.CompanyId
            })
            .ToListAsync();

        return Ok(permissions);
    }

    /// <summary>Get all available reports for dropdown</summary>
    [HttpGet("available-reports")]
    public async Task<IActionResult> GetAvailableReports()
    {
        var reports = await _context.ReportNames
            .Where(r => r.IsActive == true && r.IsDeleted != true)
            .Include(r => r.ReportType)
                .ThenInclude(rt => rt.Module)
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.ReportCode,
                ReportTypeName = r.ReportType.ReportTypeName,
                ModuleName = r.ReportType.Module.ModuleName
            })
            .OrderBy(r => r.ModuleName)
            .ThenBy(r => r.Name)
            .ToListAsync();

        return Ok(reports);
    }

    /// <summary>Save or update a report permission (upsert)</summary>
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveUserReportPermissionDto dto)
    {
        var existing = await _context.UserReportPermissions
            .FirstOrDefaultAsync(p => p.UserId == dto.UserId && p.ReportNameId == dto.ReportNameId && p.CompanyId == dto.CompanyId);

        if (existing != null)
        {
            existing.CanView = dto.CanView;
            existing.CanExport = dto.CanExport;
            existing.CanPrint = dto.CanPrint;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var perm = new UserReportPermission
            {
                UserId = dto.UserId,
                ReportNameId = dto.ReportNameId,
                CompanyId = dto.CompanyId,
                CanView = dto.CanView,
                CanExport = dto.CanExport,
                CanPrint = dto.CanPrint,
                IsActive = true
            };
            _context.UserReportPermissions.Add(perm);
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Report permission saved." });
    }

    /// <summary>Bulk save report permissions for a user</summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkSave([FromBody] BulkSaveUserReportPermissionDto dto)
    {
        var existing = await _context.UserReportPermissions
            .Where(p => p.UserId == dto.UserId)
            .ToListAsync();

        _context.UserReportPermissions.RemoveRange(existing);

        foreach (var item in dto.Permissions)
        {
            _context.UserReportPermissions.Add(new UserReportPermission
            {
                UserId = dto.UserId,
                ReportNameId = item.ReportNameId,
                CompanyId = item.CompanyId,
                CanView = item.CanView,
                CanExport = item.CanExport,
                CanPrint = item.CanPrint,
                IsActive = true
            });
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = $"Saved {dto.Permissions.Count} report permissions." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var perm = await _context.UserReportPermissions.FindAsync(id);
        if (perm == null) return NotFound();

        _context.UserReportPermissions.Remove(perm);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class SaveUserReportPermissionDto
{
    public long UserId { get; set; }
    public long ReportNameId { get; set; }
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
}

public class BulkSaveUserReportPermissionDto
{
    public long UserId { get; set; }
    public List<SaveUserReportPermissionDto> Permissions { get; set; } = new();
}
