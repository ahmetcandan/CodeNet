using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.RabbitMQ;

internal class RabbitMqHealthCheck(IOptions<HealthCheckRabitMQSettings> config) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return new TestRabbitMqService(config).CanConnection()
                ? Task.FromResult(HealthCheckResult.Healthy($"This is RabbitMQ, standing as always. Have a good work ;) "))
                : Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, RabbitMQ is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, RabbitMQ is down :("));
        }
    }
}
