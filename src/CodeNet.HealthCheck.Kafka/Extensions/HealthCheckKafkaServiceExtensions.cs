using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Kafka.Extensions;

public static class HealthCheckKafkaServiceExtensions
{
    /// <summary>
    /// Add Kafka Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sectionName"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddKafkaHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, IConfigurationSection configurationSection, TimeSpan? timeSpan = null)
    {
        services.Configure<HealthCheckKafkaSettings>(configurationSection);
        return builder.AddCheck<KafkaHealthCheck>("kafka", HealthStatus.Unhealthy, ["kafka", "queue"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
