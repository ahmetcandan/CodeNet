using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.Kafka;

internal class KafkaHealthCheck(IOptions<HealthCheckKafkaSettings> config) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var testConnection = new TestKafkaService(config);
            return testConnection.CanConnection()
                ? Task.FromResult(HealthCheckResult.Healthy($"This is Kafka, standing as always. Have a good work ;) "))
                : Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, Kafka is down :("));
        }
        catch
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"Sorry, Kafka is down :("));
        }
    }
}
