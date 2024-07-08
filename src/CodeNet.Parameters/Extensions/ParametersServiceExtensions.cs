using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Redis.Extensions;
using Microsoft.Extensions.Hosting;
using CodeNet.Parameters.Manager;

namespace CodeNet.Parameters.Extensions;

public static class ParametersServiceExtensions
{
    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddParameters(this IHostApplicationBuilder webBuilder, string connectionName, string redisSectionName)
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
    public static IHostApplicationBuilder AddParameters<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName, string redisSectionName)
        where TDbContext : ParametersDbContext
    {
        return webBuilder.AddParameters<TDbContext>(builder => builder.UseSqlServer(webBuilder.Configuration, connectionName), redisSectionName);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddParameters(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName)
    {
        return webBuilder.AddParameters<ParametersDbContext>(optionsAction, redisSectionName);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddParameters<TDbContext>(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.Services.AddScoped<IParameterManager, ParameterManager>();
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        return webBuilder.AddRedisDistributedCache(redisSectionName);
    }
}
