using CodeNet.MongoDB;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.MongoDB.Extensions;

public static class HealthCheckMongoDbServiceExtensions
{
    private const string _name = "mongo-db";

    /// <summary>
    /// Add MongoDB Health Check
    /// Checks for MongoDBContext
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null) 
        => builder.AddMongoDbHealthCheck<MongoDBContext>(name, timeSpan);

    /// <summary>
    /// Add MongoDB Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMongoDbHealthCheck<TDbContext>(this IHealthChecksBuilder builder, string name = _name, TimeSpan? timeSpan = null)
        where TDbContext : MongoDBContext 
        => builder.AddCheck<MongoDbHealthCheck<TDbContext>>(name, HealthStatus.Unhealthy, [_name, "database"], timeSpan ?? TimeSpan.FromSeconds(5));

    /// <summary>
    /// Add MongoDB Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null) 
        => builder.AddMongoDbHealthCheck(configuration.Get<MongoDbOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);

    /// <summary>
    /// Add MongoDB Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder, MongoDbOptions options, string name = _name, TimeSpan? timeSpan = null)
    {
        builder.AddCheck(name, (cancellationToken) =>
        {
            var mongoDbHealthCheck = new MongoDbHealthCheck(options);
            return mongoDbHealthCheck.CheckHealthAsync(null, cancellationToken).Result;
        }, [_name, "database"], timeSpan ?? TimeSpan.FromSeconds(5));
        return builder;
    }
}
