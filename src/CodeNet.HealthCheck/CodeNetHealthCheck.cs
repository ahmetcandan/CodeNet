using CodeNet.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck;

internal class CodeNetHealthCheck(IIdentityContext identityContext) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var requestId = identityContext.RequestId;
            if (requestId != Guid.Empty)
                return Task.FromResult(HealthCheckResult.Healthy("This is CodeNet, standing as always. Have a good work ;) "));

            return Task.FromResult(HealthCheckResult.Unhealthy("Sorry, CodeNet is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Sorry, CodeNet is down :("));
        }
    }
}
