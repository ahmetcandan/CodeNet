using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.EntityFramework.Extensions;

public static class EntityFrameworkServiceExtensions
{
    private const string _name = "entity-framework";

    /// <summary>
    /// Add EntityFramework Health Check
    /// Checks for DbContext
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null)
    {
        return builder.AddEntityFrameworkHealthCheck<DbContext>(name, timeSpan);
    }

    /// <summary>
    /// Add EntityFramework Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck<TDbContext>(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null)
        where TDbContext : DbContext
    {
        return builder.AddCheck<EntityFrameworkHealthCheck<TDbContext>>(name, HealthStatus.Unhealthy, ["sql", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Add EntityFramework Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
    {
        return builder.AddEntityFrameworkHealthCheck(configuration.Get<DbContextOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);
    }

    /// <summary>
    /// Add EntityFramework Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddEntityFrameworkHealthCheck(this IHealthChecksBuilder builder, DbContextOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.AddCheck(name, (cancellationToken) =>
        {
            var efHealthCheck = new EntityFrameworkHealthCheck(options);
            return efHealthCheck.CheckHealthAsync(null, cancellationToken).Result;
        }, ["sql", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
        return builder;
    }
}
