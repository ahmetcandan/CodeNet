using CodeNet.Redis.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodeNet.HealthCheck.Redis;

internal class RedisHealthCheck(IOptions<RedisSettings> redisSettings) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = new EndPointCollection
                {
                    { redisSettings.Value.Hostname, redisSettings.Value.Port }
                }
            }).GetDatabase().PingAsync();
            return HealthCheckResult.Healthy("This is Redis, standing as always. Have a good work ;) ");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, Redis is down :(");
        }
    }
}
