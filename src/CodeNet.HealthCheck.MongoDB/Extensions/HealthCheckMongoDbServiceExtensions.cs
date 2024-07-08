using CodeNet.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.MongoDB.Extensions;

public static class HealthCheckMongoDbServiceExtensions
{
    /// <summary>
    /// Add MongoDB Health Check
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMongoDbHealthCheck<TDbContext>(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
        where TDbContext : MongoDBContext
    {
        return builder.AddCheck<MongoDbHealthCheck<TDbContext>>("mongo-db", HealthStatus.Unhealthy, ["mongo-db", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Add MongoDB Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<MongoDbHealthCheck<MongoDBContext>>("mongo-db", HealthStatus.Unhealthy, ["mongo-db", "database"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
