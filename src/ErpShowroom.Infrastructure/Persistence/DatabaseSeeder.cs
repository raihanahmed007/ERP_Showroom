using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.sys.Entities;

namespace ErpShowroom.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedCoreAsync(context);
        await SeedTestDataFromScriptAsync(context);
    }

    private static async Task SeedCoreAsync(AppDbContext context)
    {
        // 1. Core Roles
        var roles = new[] { "SuperAdmin", "FinanceManager", "Collector", "WorkshopTech", "HRManager" };
        foreach (var r in roles)
        {
            if (!await context.Roles.AnyAsync(x => x.RoleName == r))
                context.Roles.Add(new Role { RoleName = r, RoleCode = r.ToUpper(), IsActive = true });
        }
        await context.SaveChangesAsync();

        // 2. Generic Permissions
        var modules = new[] { "sys", "acc", "fin", "inv", "prc", "crm", "wrk", "hr", "prl", "doc", "bank", "wf" };
        var actions = new[] { "create", "read", "update", "delete", "approve" };

        foreach (var m in modules)
        {
            foreach (var a in actions)
            {
                var key = $"{m}.{a}";
                if (!await context.Permissions.AnyAsync(x => x.PermissionKey == key))
                {
                    context.Permissions.Add(new Permission { ModuleName = m, ActionName = a, PermissionKey = key, IsActive = true });
                }
            }
        }

        // Module-specific overrides
        var specificPerms = new[]
        {
            new { Module = "fin", Action = "agreements.approve", Key = "fin.agreements.approve" },
            new { Module = "fin", Action = "emi.collect", Key = "fin.emi.collect" }
        };
        foreach (var sp in specificPerms)
        {
            if (!await context.Permissions.AnyAsync(x => x.PermissionKey == sp.Key))
            {
                context.Permissions.Add(new Permission { ModuleName = sp.Module, ActionName = sp.Action, PermissionKey = sp.Key, IsActive = true });
            }
        }
        await context.SaveChangesAsync();

        // 3. Connect SuperAdmin to all Permissions securely
        var superAdmin = await context.Roles.FirstAsync(r => r.RoleName == "SuperAdmin");
        var allPerms = await context.Permissions.ToListAsync();

        foreach (var p in allPerms)
        {
            if (!await context.RolePermissions.AnyAsync(rp => rp.RoleId == superAdmin.Id && rp.PermissionId == p.Id))
                context.RolePermissions.Add(new RolePermission { RoleId = superAdmin.Id, PermissionId = p.Id, IsActive = true });
        }
        await context.SaveChangesAsync();

        // 4. Provision Root Developer User mapped with BCrypt
        if (!await context.Users.AnyAsync(u => u.UserName == "admin"))
        {
            var adminUser = new User
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@himumotors.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", 12), // Seed Hashed Password with work factor 12
                IsActive = true
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            context.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = superAdmin.Id, IsActive = true });
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedTestDataFromScriptAsync(AppDbContext context)
    {
        const string seedFileName = "seed_all_tables.sql";
        var scriptPath = Path.Combine(AppContext.BaseDirectory, seedFileName);
        if (!File.Exists(scriptPath))
        {
            scriptPath = Path.Combine(Directory.GetCurrentDirectory(), seedFileName);
        }

        if (!File.Exists(scriptPath))
            return;

        var sql = await File.ReadAllTextAsync(scriptPath);
        if (string.IsNullOrWhiteSpace(sql))
            return;

        await context.Database.ExecuteSqlRawAsync(sql);
    }
}
