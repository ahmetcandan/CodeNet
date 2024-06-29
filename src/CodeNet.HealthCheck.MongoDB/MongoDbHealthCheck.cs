using CodeNet.MongoDB;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.MongoDB;

internal class MongoDbHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck
    where TDbContext : MongoDBContext
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await dbContext.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy($"This is MongoDB, standing as always. Have a good work ;) ");

            return HealthCheckResult.Unhealthy($"Sorry, MongoDB is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy($"Sorry, MongoDB is down :(");
        }
    }
}
