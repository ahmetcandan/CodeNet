using CodeNet.Transport.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.Transport;

internal class TransportHealthCheck(IOptions<TransportHealthCheckOptions> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return await new DataTransferClient(options.Value.HostName, options.Value.Port, "healthcheck").CanConnectionAsync()
                ? HealthCheckResult.Healthy("This is CodeNet.Transport, standing as always. Have a good work ;) ")
                : HealthCheckResult.Unhealthy("Sorry, CodeNet.Transport is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, CodeNet.Transport is down :(");
        }
    }
}
