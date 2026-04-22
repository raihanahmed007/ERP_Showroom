using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Infrastructure.Persistence;

namespace ErpShowroom.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed Companies (Better to use the named DbSet 'Companies')
        var mainCompany = await context.Companies.FirstOrDefaultAsync(c => c.CompanyCode == "ERPSHOW");
        if (mainCompany == null)
        {
            mainCompany = new Company
            {
                CompanyName = "ERP Showroom Demo",
                CompanyCode = "ERPSHOW",
                CompanyType = CompanyType.Main,
                IsHeadOffice = true,
                MaxUsers = 100,
                IsActive = true
            };
            context.Companies.Add(mainCompany);
            await context.SaveChangesAsync();
        }

        // Seed Roles or update known role entries if they already exist.
        var sysAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleCode == "SYSADMIN");
        if (sysAdminRole == null)
        {
            sysAdminRole = new Role
            {
                RoleName = "System Administrator",
                RoleCode = "SYSADMIN",
                RoleType = RoleType.SystemAdmin,
                IsActive = true
            };
            context.Roles.Add(sysAdminRole);
        }
        else
        {
            sysAdminRole.RoleName = "System Administrator";
            sysAdminRole.RoleType = RoleType.SystemAdmin;
            sysAdminRole.IsActive = true;
        }

        var normalUserRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleCode == "USER");
        if (normalUserRole == null)
        {
            normalUserRole = new Role
            {
                RoleName = "User",
                RoleCode = "USER",
                RoleType = RoleType.User,
                IsActive = true
            };
            context.Roles.Add(normalUserRole);
        }
        else
        {
            normalUserRole.RoleName = "User";
            normalUserRole.RoleType = RoleType.User;
            normalUserRole.IsActive = true;
        }

        await context.SaveChangesAsync();

        // Seed Users or update known test users if they already exist.
        var adminUser = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == "admin" || u.Email == "admin@erpshowroom.com");

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@erpshowroom.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", 12),
                EmailConfirmed = true,
                IsActive = true
            };
            context.Users.Add(adminUser);
        }
        else
        {
            adminUser.UserName = "admin";
            adminUser.NormalizedUserName = "ADMIN";
            adminUser.Email = "admin@erpshowroom.com";
            adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", 12);
            adminUser.EmailConfirmed = true;
            adminUser.IsActive = true;
        }

        var testUser = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == "testuser" || u.Email == "test@erpshowroom.com" || u.Phone == "1234567890");

        if (testUser == null)
        {
            testUser = new User
            {
                UserName = "testuser",
                NormalizedUserName = "TESTUSER",
                Email = "test@erpshowroom.com",
                Phone = "1234567890",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!", 12),
                EmailConfirmed = true,
                PhoneConfirmed = true,
                IsActive = true
            };
            context.Users.Add(testUser);
        }
        else
        {
            testUser.UserName = "testuser";
            testUser.NormalizedUserName = "TESTUSER";
            testUser.Email = "test@erpshowroom.com";
            testUser.Phone = "1234567890";
            testUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!", 12);
            testUser.EmailConfirmed = true;
            testUser.PhoneConfirmed = true;
            testUser.IsActive = true;
        }

        await context.SaveChangesAsync();

        // Assign roles to known users if needed
        var adminRoleForUsers = await context.Roles.FirstAsync(r => r.RoleCode == "SYSADMIN");
        var userRoleForUsers = await context.Roles.FirstAsync(r => r.RoleCode == "USER");

        if (!await context.UserRoles.AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRoleForUsers.Id))
        {
            context.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRoleForUsers.Id });
        }

        if (!await context.UserRoles.AnyAsync(ur => ur.UserId == testUser.Id && ur.RoleId == userRoleForUsers.Id))
        {
            context.UserRoles.Add(new UserRole { UserId = testUser.Id, RoleId = userRoleForUsers.Id });
        }

        await context.SaveChangesAsync();

        // Seed Permissions
        var defaultPermissions = new[]
        {
            new Permission { ModuleName = "System", ActionName = "Login", PermissionKey = "sys.login" },
            new Permission { ModuleName = "System", ActionName = "Logout", PermissionKey = "sys.logout" },
            new Permission { ModuleName = "System", ActionName = "ChangePassword", PermissionKey = "sys.changepassword" },
            new Permission { ModuleName = "User", ActionName = "View", PermissionKey = "user.view" },
            new Permission { ModuleName = "User", ActionName = "Create", PermissionKey = "user.create" }
        };

        foreach (var permissionDef in defaultPermissions)
        {
            var existingPermission = await context.Permissions.FirstOrDefaultAsync(p => p.PermissionKey == permissionDef.PermissionKey);
            if (existingPermission == null)
            {
                context.Permissions.Add(permissionDef);
            }
            else
            {
                existingPermission.ModuleName = permissionDef.ModuleName;
                existingPermission.ActionName = permissionDef.ActionName;
                existingPermission.PermissionKey = permissionDef.PermissionKey;
            }
        }

        await context.SaveChangesAsync();

        // Seed Role Permissions
        var adminRoleForPermissions = await context.Roles.FirstAsync(r => r.RoleCode == "SYSADMIN");
        var userRoleForPermissions = await context.Roles.FirstAsync(r => r.RoleCode == "USER");
        var permissions = await context.Permissions.ToListAsync();

        foreach (var permission in permissions)
        {
            if (!await context.RolePermissions.AnyAsync(rp => rp.RoleId == adminRoleForPermissions.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = adminRoleForPermissions.Id,
                    PermissionId = permission.Id
                });
            }

            if (permission.PermissionKey != "user.create" && !await context.RolePermissions.AnyAsync(rp => rp.RoleId == userRoleForPermissions.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = userRoleForPermissions.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await context.SaveChangesAsync();

        // Seed System Infrastructure (Modules, Features, Pages)
        if (!await context.Modules.AnyAsync())
        {
            var sysModule = new AppModule { ModuleName = "System Identity", ModuleCode = "SYS", SortOrder = 1 };
            context.Modules.Add(sysModule);
            await context.SaveChangesAsync();

            var userFeature = new Feature { ModuleId = sysModule.Id, FeatureName = "User Management", FeatureCode = "USER_MGMT", SortOrder = 1, FeatureType = FeatureType.Page };
            context.Features.Add(userFeature);
            await context.SaveChangesAsync();

            var usersPage = new AppPage { FeatureId = userFeature.Id, PageName = "Users", PageCode = "USERS_PAGE", RouteUrl = "/sys/users", SortOrder = 1 };
            var rolesPage = new AppPage { FeatureId = userFeature.Id, PageName = "Roles", PageCode = "ROLES_PAGE", RouteUrl = "/sys/roles", SortOrder = 2 };
            context.Pages.AddRange(usersPage, rolesPage);
            await context.SaveChangesAsync();
        }
    }
}