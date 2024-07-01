using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Redis.Extensions;

namespace CodeNet.Parameters.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, string connectionName, string redisSectionName)
    {
        return webBuilder.AddParameters<ParametersDbContext>(connectionName, redisSectionName);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName, string redisSectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.AddDbContext<TDbContext>(connectionName);
        return webBuilder.AddRedisDistributedCache(redisSectionName);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName)
    {
        webBuilder.AddParameters<ParametersDbContext>(optionsAction, redisSectionName);
        return webBuilder.AddRedisDistributedCache(redisSectionName);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        return webBuilder.AddRedisDistributedCache(redisSectionName);
    }
}
