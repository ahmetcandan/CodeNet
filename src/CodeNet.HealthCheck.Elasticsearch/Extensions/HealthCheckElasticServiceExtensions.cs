using CodeNet.Elasticsearch;
using CodeNet.Elasticsearch.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Elasticsearch.Extensions;

public static class HealthCheckElasticServiceExtensions
{
    private const string _name = "elasticsearch";

    /// <summary>
    /// Add Elasticsearch Health Check
    /// Checks for ElasticsearchDbContext
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null) 
        => builder.AddElasticsearchHealthCheck<ElasticsearchDbContext>(name, timeSpan);

    /// <summary>
    /// Add Elasticsearch Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck<TDbContext>(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null)
        where TDbContext : ElasticsearchDbContext 
        => builder.AddCheck<ElasticsearchHealthCheck<TDbContext>>(name, HealthStatus.Unhealthy, ["elasticsearch"], timeSpan ?? TimeSpan.FromSeconds(5));

    /// <summary>
    /// Add Elasticsearch Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null) 
        => builder.AddElasticsearchHealthCheck(configuration.Get<ElasticsearchOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add Elasticsearch Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddElasticsearchHealthCheck(this IHealthChecksBuilder builder, ElasticsearchOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.AddCheck(name, (cancellationToken) =>
        {
            var elasticsearchHealthCheck = new ElasticsearchHealthCheck(options);
            return elasticsearchHealthCheck.CheckHealthAsync(null, cancellationToken).Result;
        }, ["elasticsearch"], timeSpan ?? TimeSpan.FromSeconds(5));
        return builder;
    }
}
