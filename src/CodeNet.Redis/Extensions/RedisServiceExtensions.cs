using CodeNet.Redis.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;

namespace CodeNet.Redis.Extensions;

public static class RedisServiceExtensions
{
    /// <summary>
    /// Use Distributed Cache
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDistributedCache(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CacheMiddleware?>();
    }

    /// <summary>
    /// Use Distributed Lock
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDistributedLock(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LockMiddleware?>();
    }

    /// <summary>
    /// Add Redis Distributed Cache
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHostApplicationBuilder AddRedisDistributedCache(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings?>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        webBuilder.Services.AddStackExchangeRedisCache(option =>
         {
             option.Configuration = $"{redisSettings.Hostname}:{redisSettings.Port}";
             option.InstanceName = redisSettings.InstanceName;
         });
        webBuilder.Services.AddScoped(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        return webBuilder;
    }

    /// <summary>
    /// Add Redis Distributed Lock
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHostApplicationBuilder AddRedisDistributedLock(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings?>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        var ipAddresses = Dns.GetHostAddresses(redisSettings.Hostname);
        var endPoints = new List<RedLockEndPoint?>
        {
            new() { EndPoint = new IPEndPoint(ipAddresses[0], redisSettings.Port) }
        };
        webBuilder.Services.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));
        return webBuilder;
    }
}
