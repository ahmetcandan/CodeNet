using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodeNet.HealthCheck.Redis;

internal class RedisHealthCheck(IOptions<RedisHealthCheckOptions> redisSettings) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await ConnectionMultiplexer.Connect(redisSettings.Value.Configuration).GetDatabase().PingAsync();
            return HealthCheckResult.Healthy("This is Redis, standing as always. Have a good work ;) ");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, Redis is down :(");
        }
    }
}
