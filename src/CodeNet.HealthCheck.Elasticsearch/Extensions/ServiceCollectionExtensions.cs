using CodeNet.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Elasticsearch.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Elasticsearch Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck<TDbContext>(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
        where TDbContext : ElasticsearchDbContext
    {
        return builder.AddCheck<ElasticsearchHealthCheck<TDbContext>>("elasticsearch", HealthStatus.Unhealthy, ["elasticsearch"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Add Elasticsearch Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<ElasticsearchHealthCheck<ElasticsearchDbContext>>("elasticsearch", HealthStatus.Unhealthy, ["elasticsearch"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
