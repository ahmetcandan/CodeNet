﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Redis Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder, TimeSpan? timeSpan = null)
    {
        return builder.AddCheck<RedisHealthCheck>("redis", HealthStatus.Unhealthy, ["redis"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
