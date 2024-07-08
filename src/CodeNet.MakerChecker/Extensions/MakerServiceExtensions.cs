using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Core.Extensions;

namespace CodeNet.MakerChecker.Extensions;

public static class MakerServiceExtensions
{
    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker(this IServiceCollection services, string connectionString)
    {
        return services.AddMakerChecker<MakerCheckerDbContext>(connectionString);
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : MakerCheckerDbContext
    {
        return services.AddMakerChecker<TDbContext>(builder => builder.UseSqlServer(connectionString));
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        return services.AddMakerChecker<MakerCheckerDbContext>(optionsAction);
    }

    /// <summary>
    /// Add Maker Checker
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddMakerChecker<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : MakerCheckerDbContext
    {
        services.AddDbContext<TDbContext>(optionsAction);
        services.AddScoped<IMakerCheckerManager, MakerCheckerManager>();
        return services.AddCodeNetContext();
    }
}
