using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add CodeNet Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddCodeNetHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder
            .AddCheck<CodeNetHealthCheck>("codenet", HealthStatus.Unhealthy, ["codenet", "identity-context"], timeSpan ?? TimeSpan.FromSeconds(5))
            .AddCheck<MediatorHealthCheck>("mediator", HealthStatus.Unhealthy, ["mediator"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Use Health Check
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static WebApplication UseHealthChecks(this WebApplication app, string pattern)
    {
        app.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}
