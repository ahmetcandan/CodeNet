using CodeNet.Core.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Extensions;

public static class HealthCheckServiceExtensions
{
    /// <summary>
    /// Add CodeNet Health Check
    /// </summary>
    /// <param name="services"></param>
    /// <param name="action"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthChecksBuilder> action)
    {
        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        var healthCheckBuilder = HealthCheckServiceCollectionExtensions.AddHealthChecks(services);
        action(healthCheckBuilder);
        return services;
    }

    /// <summary>
    /// Add CodeNet Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddCodeNetHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<CodeNetHealthCheck>("codenet", HealthStatus.Unhealthy, ["codenet", "identity-context"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    public static IHealthChecksBuilder AddHealthCheck<THealthCheck>(this IHealthChecksBuilder builder, string name, IEnumerable<string> tags, HealthStatus failureStatus = HealthStatus.Unhealthy, TimeSpan? timeSpan = null)
        where THealthCheck : class, IHealthCheck
    {
        return builder.AddCheck<THealthCheck>(name, failureStatus, tags);
    }

    /// <summary>
    /// Use Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UseCodeNetHealthChecks(this IEndpointRouteBuilder builder, string pattern = "/health")
    {
        builder.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return builder;
    }
}
