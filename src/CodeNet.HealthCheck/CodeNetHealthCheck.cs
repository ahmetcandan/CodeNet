using CodeNet.Core.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck;

internal class CodeNetHealthCheck(ICodeNetContext identityContext) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return string.IsNullOrEmpty(identityContext.CorrelationId)
                ? Task.FromResult(HealthCheckResult.Healthy("This is CodeNet, standing as always. Have a good work ;) "))
                : Task.FromResult(HealthCheckResult.Unhealthy("Sorry, CodeNet is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Sorry, CodeNet is down :("));
        }
    }
}
