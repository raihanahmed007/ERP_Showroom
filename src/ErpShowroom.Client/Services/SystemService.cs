using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErpShowroom.Client.Services;

// ==================== DTOs ====================
public class UserListDto
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? EmployeeId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int? FailedLoginCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<UserRoleListItemDto> Roles { get; set; } = new();
}

public class UserRoleListItemDto
{
    public long Id { get; set; }
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
}

public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? EmployeeId { get; set; }
}

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public long? EmployeeId { get; set; }
    public bool? IsActive { get; set; }
}

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}

public class RoleDto
{
    public long Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string? RoleType { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class RoleCreateRequest
{
    public string RoleName { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public int RoleType { get; set; }
    public string? Description { get; set; }
}

public class UserRoleAssignmentDto
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? RoleType { get; set; }
    public bool? IsActive { get; set; }
}

public class AvailableRoleDto
{
    public long Id { get; set; }
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
    public string? RoleType { get; set; }
    public string? Description { get; set; }
}

public class AssignRoleRequest
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}

public class AvailablePageDto
{
    public long Id { get; set; }
    public string? PageName { get; set; }
    public string? PageCode { get; set; }
    public string? RouteUrl { get; set; }
    public string? ModuleName { get; set; }
    public string? FeatureName { get; set; }
    public string? PageType { get; set; }
}

public class UserPageAccessDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PageId { get; set; }
    public string? PageName { get; set; }
    public string? PageRoute { get; set; }
    public string? ModuleName { get; set; }
    public string? FeatureName { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
    public bool OverridesRole { get; set; }
    public long CompanyId { get; set; }
}

public class SaveUserPageAccessRequest
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

public class BulkSaveUserPageAccessRequest
{
    public long UserId { get; set; }
    public List<SaveUserPageAccessRequest> Accesses { get; set; } = new();
}

public class PagePermissionDto
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long PageId { get; set; }
    public string? PageName { get; set; }
    public string? PageRoute { get; set; }
    public string? ModuleName { get; set; }
    public string? FeatureName { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanApprove { get; set; }
    public bool CanExport { get; set; }
    public long CompanyId { get; set; }
}

public class SavePagePermissionRequest
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

public class BulkSavePagePermissionsRequest
{
    public long RoleId { get; set; }
    public List<SavePagePermissionRequest> Permissions { get; set; } = new();
}

// ==================== Service Interface ====================
public interface ISystemService
{
    // Users
    Task<List<UserListDto>> GetUsersAsync();
    Task<UserListDto?> GetUserByIdAsync(long id);
    Task<bool> CreateUserAsync(CreateUserRequest request);
    Task<bool> UpdateUserAsync(long id, UpdateUserRequest request);
    Task<bool> ResetPasswordAsync(long id, string newPassword);
    Task<bool> UnlockUserAsync(long id);
    Task<bool> DeleteUserAsync(long id);

    // Roles
    Task<List<RoleDto>> GetRolesAsync();
    Task<bool> CreateRoleAsync(RoleCreateRequest request);
    Task<bool> UpdateRoleAsync(long id, RoleCreateRequest request);
    Task<bool> DeleteRoleAsync(long id);

    // User Role Assignment
    Task<List<UserRoleAssignmentDto>> GetUserRolesAsync(long userId);
    Task<List<AvailableRoleDto>> GetAvailableRolesAsync();
    Task<bool> AssignRoleAsync(long userId, long roleId);
    Task<bool> RemoveRoleAssignmentAsync(long id);

    // User Page Access (Overrides)
    Task<List<UserPageAccessDto>> GetUserPageAccessAsync(long userId);
    Task<List<AvailablePageDto>> GetAvailablePagesAsync();
    Task<bool> SaveUserPageAccessAsync(SaveUserPageAccessRequest request);
    Task<bool> BulkSaveUserPageAccessAsync(long userId, List<SaveUserPageAccessRequest> accesses);
    Task<bool> DeleteUserPageAccessAsync(long id);

    // Role Page Permissions
    Task<List<PagePermissionDto>> GetRolePagePermissionsAsync(long roleId);
    Task<bool> SaveRolePagePermissionAsync(SavePagePermissionRequest request);
    Task<bool> BulkSaveRolePagePermissionsAsync(long roleId, List<SavePagePermissionRequest> permissions);

    // User Report Permissions
    Task<List<UserReportPermissionDto>> GetUserReportPermissionsAsync(long userId);
    Task<List<AvailableReportDto>> GetAvailableReportsAsync();
    Task<bool> SaveUserReportPermissionAsync(SaveUserReportPermissionRequest request);
    Task<bool> BulkSaveUserReportPermissionsAsync(long userId, List<SaveUserReportPermissionRequest> permissions);
    Task<bool> DeleteUserReportPermissionAsync(long id);
}

// ==================== Service Implementation ====================
public class SystemService : ISystemService
{
    private readonly IApiClient _api;

    public SystemService(IApiClient api)
    {
        _api = api;
    }

    // ---------- Users ----------
    public async Task<List<UserListDto>> GetUsersAsync()
        => await _api.GetAsync<List<UserListDto>>("api/users") ?? new();

    public async Task<UserListDto?> GetUserByIdAsync(long id)
        => await _api.GetAsync<UserListDto>($"api/users/{id}");

    public async Task<bool> CreateUserAsync(CreateUserRequest request)
        => await _api.PostAsyncBool("api/users", request);

    public async Task<bool> UpdateUserAsync(long id, UpdateUserRequest request)
        => await _api.PutAsync($"api/users/{id}", request);

    public async Task<bool> ResetPasswordAsync(long id, string newPassword)
        => await _api.PutAsync($"api/users/{id}/reset-password", new ResetPasswordRequest { NewPassword = newPassword });

    public async Task<bool> UnlockUserAsync(long id)
        => await _api.PutAsync($"api/users/{id}/unlock", new { });

    public async Task<bool> DeleteUserAsync(long id)
        => await _api.DeleteAsync($"api/users/{id}");

    // ---------- Roles ----------
    public async Task<List<RoleDto>> GetRolesAsync()
        => await _api.GetAsync<List<RoleDto>>("api/roles") ?? new();

    public async Task<bool> CreateRoleAsync(RoleCreateRequest request)
        => await _api.PostAsyncBool("api/roles", request);

    public async Task<bool> UpdateRoleAsync(long id, RoleCreateRequest request)
        => await _api.PutAsync($"api/roles/{id}", request);

    public async Task<bool> DeleteRoleAsync(long id)
        => await _api.DeleteAsync($"api/roles/{id}");

    // ---------- User Roles ----------
    public async Task<List<UserRoleAssignmentDto>> GetUserRolesAsync(long userId)
        => await _api.GetAsync<List<UserRoleAssignmentDto>>($"api/userroles/user/{userId}") ?? new();

    public async Task<List<AvailableRoleDto>> GetAvailableRolesAsync()
        => await _api.GetAsync<List<AvailableRoleDto>>("api/userroles/available-roles") ?? new();

    public async Task<bool> AssignRoleAsync(long userId, long roleId)
        => await _api.PostAsyncBool("api/userroles", new AssignRoleRequest { UserId = userId, RoleId = roleId });

    public async Task<bool> RemoveRoleAssignmentAsync(long id)
        => await _api.DeleteAsync($"api/userroles/{id}");

    // ---------- User Page Access ----------
    public async Task<List<UserPageAccessDto>> GetUserPageAccessAsync(long userId)
        => await _api.GetAsync<List<UserPageAccessDto>>($"api/userpageaccess/user/{userId}") ?? new();

    public async Task<List<AvailablePageDto>> GetAvailablePagesAsync()
        => await _api.GetAsync<List<AvailablePageDto>>("api/userpageaccess/available-pages") ?? new();

    public async Task<bool> SaveUserPageAccessAsync(SaveUserPageAccessRequest request)
        => await _api.PostAsyncBool("api/userpageaccess", request);

    public async Task<bool> BulkSaveUserPageAccessAsync(long userId, List<SaveUserPageAccessRequest> accesses)
        => await _api.PostAsyncBool("api/userpageaccess/bulk", new BulkSaveUserPageAccessRequest { UserId = userId, Accesses = accesses });

    public async Task<bool> DeleteUserPageAccessAsync(long id)
        => await _api.DeleteAsync($"api/userpageaccess/{id}");

    // ---------- Role Page Permissions ----------
    public async Task<List<PagePermissionDto>> GetRolePagePermissionsAsync(long roleId)
        => await _api.GetAsync<List<PagePermissionDto>>($"api/pagepermissions/role/{roleId}") ?? new();

    public async Task<bool> SaveRolePagePermissionAsync(SavePagePermissionRequest request)
        => await _api.PostAsyncBool("api/pagepermissions", request);

    public async Task<bool> BulkSaveRolePagePermissionsAsync(long roleId, List<SavePagePermissionRequest> permissions)
        => await _api.PostAsyncBool("api/pagepermissions/bulk", new BulkSavePagePermissionsRequest { RoleId = roleId, Permissions = permissions });

    // ---------- User Report Permissions ----------
    public async Task<List<UserReportPermissionDto>> GetUserReportPermissionsAsync(long userId)
        => await _api.GetAsync<List<UserReportPermissionDto>>($"api/userreportpermissions/user/{userId}") ?? new();

    public async Task<List<AvailableReportDto>> GetAvailableReportsAsync()
        => await _api.GetAsync<List<AvailableReportDto>>("api/userreportpermissions/available-reports") ?? new();

    public async Task<bool> SaveUserReportPermissionAsync(SaveUserReportPermissionRequest request)
        => await _api.PostAsyncBool("api/userreportpermissions", request);

    public async Task<bool> BulkSaveUserReportPermissionsAsync(long userId, List<SaveUserReportPermissionRequest> permissions)
        => await _api.PostAsyncBool("api/userreportpermissions/bulk", new { UserId = userId, Permissions = permissions });

    public async Task<bool> DeleteUserReportPermissionAsync(long id)
        => await _api.DeleteAsync($"api/userreportpermissions/{id}");
}

// ==================== Report Permission DTOs ====================
public class UserReportPermissionDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long ReportNameId { get; set; }
    public string? ReportNameText { get; set; }
    public string? ReportCode { get; set; }
    public string? ReportTypeName { get; set; }
    public string? ModuleName { get; set; }
    public bool CanView { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
    public long CompanyId { get; set; }
}

public class AvailableReportDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? ReportCode { get; set; }
    public string? ReportTypeName { get; set; }
    public string? ModuleName { get; set; }
}

public class SaveUserReportPermissionRequest
{
    public long UserId { get; set; }
    public long ReportNameId { get; set; }
    public long CompanyId { get; set; }
    public bool CanView { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
}

