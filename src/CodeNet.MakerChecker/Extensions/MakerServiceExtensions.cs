using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CodeNet.MakerChecker.Extensions;

public static class MakerServiceExtensions
{
    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMakerChecker(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddMakerChecker<MakerCheckerDbContext>(connectionName);
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMakerChecker<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName)
        where TDbContext : MakerCheckerDbContext
    {
        return webBuilder.AddMakerChecker<TDbContext>(builder => builder.UseSqlServer(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMakerChecker(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
    {
        return webBuilder.AddMakerChecker<MakerCheckerDbContext>(optionsAction);
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMakerChecker<TDbContext>(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : MakerCheckerDbContext
    {
        webBuilder.AddDbContext<TDbContext>(optionsAction);
        webBuilder.Services.AddScoped<IMakerCheckerManager, MakerCheckerManager>();
        return webBuilder;
    }
}
