using CodeNet.HealthCheck.MongoDB;
using CodeNet.RabbitMQ.Settings;
using Microsoft.AspNetCore.Builder;
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
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(this IHealthChecksBuilder builder, WebApplicationBuilder webBuilder, string sectionName, TimeSpan? timeSpan = null)
    {
        webBuilder.Services.Configure<BaseRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        return builder.AddCheck<RabbitMqHealthCheck>("rabbit-mq", HealthStatus.Unhealthy, ["rabbit-mq", "queue"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
