using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.HealthCheck.Transport.Extensions;

public static class HealthCheckServiceExtensions
{
    private const string _name = "codenet-transport";

    /// <summary>
    /// Add CodeNet.Transport Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddCodeNetTransportHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
        => builder.AddCodeNetTransportHealthCheck(configuration.Get<TransportHealthCheckOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add CodeNet.Transport Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddCodeNetTransportHealthCheck(this IHealthChecksBuilder builder, TransportHealthCheckOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.Services.Configure<TransportHealthCheckOptions>(c =>
        {
            c.HostName = options.HostName;
            c.Port = options.Port;
        });
        return builder.AddCheck<TransportHealthCheck>(name, Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, [_name], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
