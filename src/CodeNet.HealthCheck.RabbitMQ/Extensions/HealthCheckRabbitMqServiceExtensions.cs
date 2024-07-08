using CodeNet.HealthCheck.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.RabbitMQ.Extensions;

public static class HealthCheckRabbitMqServiceExtensions
{
    /// <summary>
    /// Add RabbitMQ Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sectionName"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, IConfigurationSection configurationSection, TimeSpan? timeSpan = null)
    {
        services.Configure<HealthCheckRabitMQSettings>(configurationSection);
        return builder.AddCheck<RabbitMqHealthCheck>("rabbit-mq", HealthStatus.Unhealthy, ["rabbit-mq", "queue"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
