using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add EntityFramework Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck<TDbContext>(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
        where TDbContext : DbContext
    {
        return builder.AddCheck<EntityFrameworkHealthCheck<TDbContext>>("entity-framework", HealthStatus.Unhealthy, ["sql", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Add EntityFramework Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<EntityFrameworkHealthCheck<DbContext>>("entity-framework", HealthStatus.Unhealthy, ["sql", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
