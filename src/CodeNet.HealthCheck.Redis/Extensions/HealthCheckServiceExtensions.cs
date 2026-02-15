using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.HealthCheck.Redis.Extensions;

public static class HealthCheckServiceExtensions
{
    private const string _name = "redis";

    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
        => builder.AddRedisHealthCheck(configuration.Get<RedisHealthCheckOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, RedisHealthCheckOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.Services.Configure<RedisHealthCheckOptions>(c =>
        {
            c.Configuration = options.Configuration;
        });
        return builder.AddCheck<RedisHealthCheck>(name, Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, [_name], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
