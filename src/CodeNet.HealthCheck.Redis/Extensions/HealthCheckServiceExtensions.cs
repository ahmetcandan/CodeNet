using CodeNet.Redis.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.Redis.Extensions;

public static class HealthCheckServiceExtensions
{
    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, string name = "redis", TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<RedisHealthCheck>(name, HealthStatus.Unhealthy, ["redis"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = "mongo-db", TimeSpan? timeSpan = null)
    {
        return builder.AddRedisHealthCheck(configuration.Get<RedisSettings>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);
    }

    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, RedisSettings options, string name = "mongo-db", TimeSpan? timeSpan = null)
    {
        builder.AddCheck(name, (cancellationToken) =>
        {
            var mongoDbHealthCheck = new RedisHealthCheck(Options.Create(options));
            return mongoDbHealthCheck.CheckHealthAsync(null, cancellationToken).Result;
        }, ["mongo-db", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
        return builder;
    }
}
