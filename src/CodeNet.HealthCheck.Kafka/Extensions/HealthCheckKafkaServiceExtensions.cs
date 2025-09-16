using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Kafka.Extensions;

public static class HealthCheckKafkaServiceExtensions
{
    private const string _name = "kafka";

    /// <summary>
    /// Add Kafka Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sectionName"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddKafkaHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
        => builder.AddKafkaHealthCheck(services, configuration.Get<HealthCheckKafkaSettings>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add Kafka Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddKafkaHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, HealthCheckKafkaSettings options, string name = _name, TimeSpan? timeSpan = null)
    {
        services.Configure<HealthCheckKafkaSettings>(c =>
        {
            c.Config = options.Config;
            c.Topic = options.Topic;
        });
        return builder.AddCheck<KafkaHealthCheck>(name, HealthStatus.Unhealthy, [_name, "queue"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
