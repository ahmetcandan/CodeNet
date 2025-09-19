using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.MakerChecker.DbContext;
using CodeNet.MakerChecker.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.MakerChecker.Extensions;

public static class MakerServiceExtensions
{
    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker(this IServiceCollection services, string connectionString) => services.AddMakerChecker<MakerCheckerDbContext>(connectionString);

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : MakerCheckerDbContext
        => services.AddMakerChecker<TDbContext>(builder => builder.UseSqlServer(connectionString));

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions)
        => services.AddMakerChecker<MakerCheckerDbContext>(dbOptions);

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions)
        where TDbContext : MakerCheckerDbContext
    {
        services.AddDbContext<TDbContext>(dbOptions);
        services.AddScoped<IMakerCheckerManager, MakerCheckerManager<TDbContext>>();
        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        return services;
    }
}
