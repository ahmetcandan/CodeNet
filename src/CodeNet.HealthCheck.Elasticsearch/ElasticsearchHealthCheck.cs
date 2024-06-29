using CodeNet.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Elasticsearch;

internal class ElasticsearchHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck
    where TDbContext : ElasticsearchDbContext
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await dbContext.CanConnectionAsync(cancellationToken))
                return HealthCheckResult.Healthy($"This is Elasticsearch, standing as always. Have a good work ;) ");

            return HealthCheckResult.Unhealthy($"Sorry, Elasticsearch is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy($"Sorry, Elasticsearch is down :(");
        }
    }
}
