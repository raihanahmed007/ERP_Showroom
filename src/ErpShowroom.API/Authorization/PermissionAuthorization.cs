using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace ErpShowroom.API.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionKey { get; }
    public PermissionRequirement(string permissionKey) => PermissionKey = permissionKey;
}

public class PermissionAuthorizationHandler(AppDbContext dbContext, Microsoft.Extensions.Caching.Memory.IMemoryCache cache) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true) return;

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId)) return;

        string cacheKey = $"UserPermissions_{userId}";
        if (!cache.TryGetValue(cacheKey, out System.Collections.Generic.List<string>? userPermissions))
        {
            userPermissions = await dbContext.UserRoles
                .Where(ur => ur.UserId == userId && ur.Role != null && ur.User != null && ur.User.IsActive == true)
                .SelectMany(ur => dbContext.RolePermissions.Where(rp => rp.RoleId == ur.RoleId))
                .Select(rp => rp.Permission!.PermissionKey!)
                .Distinct()
                .ToListAsync();

            cache.Set(cacheKey, userPermissions, System.TimeSpan.FromMinutes(5));
        }

        if (userPermissions != null && userPermissions.Contains(requirement.PermissionKey))
        {
            context.Succeed(requirement);
        }
    }
}

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("RequirePermission:", System.StringComparison.OrdinalIgnoreCase))
        {
            var permissionName = policyName.Substring("RequirePermission:".Length);
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(permissionName));
            return policy.Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}
