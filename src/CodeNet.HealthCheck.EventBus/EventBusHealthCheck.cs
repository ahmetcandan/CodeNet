using CodeNet.EventBus.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.EventBus;

internal class EventBusHealthCheck(IOptions<EventBusHealthCheckOptions> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await new CodeNetEventBusCanConnection(options.Value.HostName, options.Value.Port).CanConnectionAsync())
                return HealthCheckResult.Healthy("This is CodeNet.EventBus, standing as always. Have a good work ;) ");
            else
                return HealthCheckResult.Unhealthy("Sorry, CodeNet.EventBus is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, CodeNet.EventBus is down :(");
        }
    }
}
