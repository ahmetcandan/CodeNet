﻿using CodeNet.Redis.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// <param name="services"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        return services.AddRedisDistributedCache(configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add Redis Distributed Cache
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        var redisSettings = configurationSection.Get<RedisSettings?>() ?? throw new ArgumentNullException($"'{configurationSection.Path}' is null or empty in appSettings.json");
        return services.AddRedisDistributedCache(redisSettings);
    }

    /// <summary>
    /// Add Redis Distributed Cache
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, RedisSettings settings)
    {
        services.Configure<RedisSettings>(c =>
        {
            c.Port = settings.Port;
            c.Hostname = settings.Hostname;
            c.InstanceName = settings.InstanceName;
        });
        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = $"{settings.Hostname}:{settings.Port}";
            option.InstanceName = settings.InstanceName;
        });
        return services.AddScoped(typeof(IDistributedCache<>), typeof(DistributedCache<>));
    }

    /// <summary>
    /// Add Redis Distributed Lock
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        return services.AddRedisDistributedLock(configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add Redis Distributed Lock
    /// </summary>
    /// <param name="services"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type RedisSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        var redisSettings = configurationSection.Get<RedisSettings?>() ?? throw new ArgumentNullException($"'{configurationSection.Path}' is null or empty in appSettings.json");
        return services.AddRedisDistributedLock(redisSettings);
    }

    /// <summary>
    /// Add Redis Distributed Lock
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, RedisSettings settings)
    {
        services.Configure<RedisSettings>(c =>
        {
            c.Port = settings.Port;
            c.Hostname = settings.Hostname;
            c.InstanceName = settings.InstanceName;
        });
        var ipAddresses = Dns.GetHostAddresses(settings.Hostname);
        var endPoints = new List<RedLockEndPoint?>
        {
            new() { EndPoint = new IPEndPoint(ipAddresses[0], settings.Port) }
        };
        return services.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));
    }
}
