using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMakerChecker(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddSqlServer<MakerCheckerDbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMakerChecker<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName)
        where TDbContext : MakerCheckerDbContext
    {
        webBuilder.AddSqlServer<TDbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMakerChecker(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
    {
        webBuilder.AddDbContext<MakerCheckerDbContext>(optionsAction);
        return webBuilder;
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMakerChecker<TDbContext>(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : MakerCheckerDbContext
    {
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        return webBuilder;
    }
}
