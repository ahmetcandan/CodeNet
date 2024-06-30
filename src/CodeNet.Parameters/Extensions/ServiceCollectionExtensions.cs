using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Redis.Extensions;
using CodeNet.Parameters.Settings;

namespace CodeNet.Parameters.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, string connectionName, string redisSectionName, string parameterSectionName)
    {
        webBuilder.Services.Configure<ParameterSettings>(webBuilder.Configuration.GetSection(parameterSectionName));
        webBuilder.AddSqlServer<ParametersDbContext>(connectionName);
        webBuilder.AddRedisDistributedCache(redisSectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName, string redisSectionName, string parameterSectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.Services.Configure<ParameterSettings>(webBuilder.Configuration.GetSection(parameterSectionName));
        webBuilder.AddSqlServer<TDbContext>(connectionName);
        webBuilder.AddRedisDistributedCache(redisSectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName, string parameterSectionName)
    {
        webBuilder.Services.Configure<ParameterSettings>(webBuilder.Configuration.GetSection(parameterSectionName));
        webBuilder.AddDbContext<ParametersDbContext>(optionsAction);
        webBuilder.AddRedisDistributedCache(redisSectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string redisSectionName, string parameterSectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.Services.Configure<ParameterSettings>(webBuilder.Configuration.GetSection(parameterSectionName));
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        webBuilder.AddRedisDistributedCache(redisSectionName);
        return webBuilder;
    }
}
