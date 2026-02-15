using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.HealthCheck.EventBus.Extensions;

public static class HealthCheckServiceExtensions
{
    private const string _name = "codenet-eventbus";

    /// <summary>
    /// Add CodeNet.EventBus Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddCodeNetEventBusHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
        => builder.AddCodeNetEventBusHealthCheck(configuration.Get<EventBusHealthCheckOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add CodeNet.EventBus Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddCodeNetEventBusHealthCheck(this IHealthChecksBuilder builder, EventBusHealthCheckOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.Services.Configure<EventBusHealthCheckOptions>(c =>
        {
            c.HostName = options.HostName;
            c.Port = options.Port;
            c.Channel = options.Channel;
        });
        return builder.AddCheck<EventBusHealthCheck>(name, Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, [_name], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
