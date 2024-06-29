using CodeNet.Redis.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;

namespace CodeNet.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Redis Distributed Cache
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddRedisDistributedCache(this WebApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.Services.Configure<RedisSettings>(webBuilder.Configuration.GetSection(sectionName));
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        webBuilder.Services.AddStackExchangeRedisCache(option =>
         {
             option.Configuration = $"{redisSettings.Hostname}:{redisSettings.Port}";
             option.InstanceName = redisSettings.InstanceName;
         });
        return webBuilder;
    }

    /// <summary>
    /// Add Redis Distributed Lock
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddRedisDistributedLock(this WebApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.Services.Configure<RedisSettings>(webBuilder.Configuration.GetSection(sectionName));
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        var ipAddresses = Dns.GetHostAddresses(redisSettings.Hostname);
        var endPoints = new List<RedLockEndPoint>
        {
            new() { EndPoint = new IPEndPoint(ipAddresses[0], redisSettings.Port) }
        };
        webBuilder.Services.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));
        return webBuilder;
    }
}
