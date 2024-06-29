using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck;

internal class MediatorHealthCheck(IMediator mediator) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (mediator is not null)
                return Task.FromResult(HealthCheckResult.Healthy("This is Mediator, standing as always. Have a good work ;) "));

            return Task.FromResult(HealthCheckResult.Unhealthy("Sorry, Mediator is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Sorry, Mediator is down :("));
        }
    }
}
