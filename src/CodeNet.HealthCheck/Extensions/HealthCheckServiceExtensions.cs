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
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddCodeNetHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<CodeNetHealthCheck>("codenet", HealthStatus.Unhealthy, ["codenet", "identity-context"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Use Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UseHealthChecks(this IEndpointRouteBuilder builder, string pattern)
    {
        builder.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return builder;
    }
}
