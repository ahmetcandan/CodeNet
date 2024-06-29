using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddSqlServer<ParametersDbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName)
        where TDbContext : ParametersDbContext
    {
        webBuilder.AddSqlServer<TDbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
    {
        webBuilder.AddDbContext<ParametersDbContext>(optionsAction);
        return webBuilder;
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddParameters<TDbContext>(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : ParametersDbContext
    {
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        return webBuilder;
    }
}
