using CodeNet.HealthCheck.RabbitMQ;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.MongoDB;

internal class RabbitMqHealthCheck(IOptions<BaseRabbitMQSettings> config) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var testConnection = new TestRabbitMqService(config);
            return testConnection.CanConnection()
                ? Task.FromResult(HealthCheckResult.Healthy($"This is RabbitMQ, standing as always. Have a good work ;) "))
                : Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, RabbitMQ is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, RabbitMQ is down :("));
        }
    }
}
