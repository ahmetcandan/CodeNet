using CodeNet.Elasticsearch;
using CodeNet.Elasticsearch.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.Elasticsearch;

internal class ElasticsearchHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck
    where TDbContext : ElasticsearchDbContext
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.CanConnectionAsync(cancellationToken)
                ? HealthCheckResult.Healthy($"This is Elasticsearch, standing as always. Have a good work ;) ")
                : HealthCheckResult.Unhealthy($"Sorry, Elasticsearch is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy($"Sorry, Elasticsearch is down :(");
        }
    }
}

internal class ElasticsearchHealthCheck(ElasticsearchOptions options) : ElasticsearchHealthCheck<ElasticsearchDbContext>(new ElasticsearchDbContext(Options.Create(options)))
{
}
