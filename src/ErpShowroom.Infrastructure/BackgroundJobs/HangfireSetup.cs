using System;
using System.Linq;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ErpShowroom.Infrastructure.BackgroundJobs.Jobs;

namespace ErpShowroom.Infrastructure.BackgroundJobs;

public class HangfireJobLoggerFilter : JobFilterAttribute, IServerFilter
{
    public void OnPerforming(PerformingContext context)
    {
        Log.Information("Hangfire Job Starting: {JobId} for {JobName}", 
            context.BackgroundJob.Id, context.BackgroundJob.Job.Method.Name);
    }

    public void OnPerformed(PerformedContext context)
    {
        if (context.Exception != null)
        {
            Log.Error(context.Exception, "Hangfire Job Failed: {JobId} for {JobName}", 
                context.BackgroundJob.Id, context.BackgroundJob.Job.Method.Name);
        }
        else
        {
            Log.Information("Hangfire Job Completed: {JobId} for {JobName}. Duration: {Duration}ms", 
                context.BackgroundJob.Id, context.BackgroundJob.Job.Method.Name, context.Duration);
        }
    }
}

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            var hasSuperAdmin = httpContext.User.HasClaim(c => 
                (c.Type == "roles" || c.Type == System.Security.Claims.ClaimTypes.Role) && 
                c.Value.Contains("SuperAdmin"));
            return hasSuperAdmin;
        }
        return false;
    }
}

public static class HangfireExtensions
{
    public static IServiceCollection AddAndUseHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? "Server=localhost;Database=ERPHangfireDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";

        if (!string.IsNullOrEmpty(configuration["Hangfire:ConnectionString"]))
        {
            connectionString = configuration["Hangfire:ConnectionString"];
        }

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString)
            .UseFilter(new HangfireJobLoggerFilter()));

        services.AddHangfireServer();
        return services;
    }

    public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
        });

        RecurringJob.AddOrUpdate<DueScanJob>("DueScanJob_Daily", job => job.ExecuteAsync(), "0 0 * * *");
        RecurringJob.AddOrUpdate<RiskBucketUpdateJob>("RiskBucketUpdateJob_Daily", job => job.ExecuteAsync(), "30 0 * * *");
        RecurringJob.AddOrUpdate<LegalEscalationJob>("LegalEscalationJob_Daily", job => job.ExecuteAsync(), "0 1 * * *");
        RecurringJob.AddOrUpdate<SalaryProcessJob>("SalaryProcessJob_Monthly", job => job.ExecuteAsync(), "0 2 25 * *");

        return app;
    }
}
