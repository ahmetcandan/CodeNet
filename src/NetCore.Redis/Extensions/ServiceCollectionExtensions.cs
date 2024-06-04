using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;

namespace NetCore.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddRedisDistributedCache(this WebApplicationBuilder webBuilder, string sectionName)
    {
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings>()!;
        webBuilder.Services.AddStackExchangeRedisCache(option =>
         {
             option.Configuration = $"{redisSettings.Hostname}:{redisSettings.Port}";
             option.InstanceName = redisSettings.InstanceName;
         });
        return webBuilder;
    }

    public static WebApplicationBuilder AddRedisDistributedLock(this WebApplicationBuilder webBuilder, string sectionName)
    {
        var redisSettings = webBuilder.Configuration.GetSection(sectionName).Get<RedisSettings>()!;
        var ipAddresses = Dns.GetHostAddresses(redisSettings.Hostname);
        var endPoints = new List<RedLockEndPoint>
        {
            new() { EndPoint = new IPEndPoint(ipAddresses[0], redisSettings.Port) }
        };
        webBuilder.Services.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));
        return webBuilder;
    }
}
