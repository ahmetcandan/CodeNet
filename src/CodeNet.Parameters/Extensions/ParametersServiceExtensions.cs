using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Redis.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.MakerChecker.Extensions;
using Microsoft.Extensions.Configuration;

namespace CodeNet.Parameters.Extensions;

public static class ParametersServiceExtensions
{
    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, string connectionString, IConfigurationSection redisSection)
    {
        return services.AddParameters<ParametersDbContext>(connectionString, redisSection);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters<TDbContext>(this IServiceCollection services, string connectionString, IConfigurationSection redisSection)
        where TDbContext : ParametersDbContext
    {
        return services.AddParameters<TDbContext>(builder => builder.UseSqlServer(connectionString), redisSection);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection redisSection)
    {
        return services.AddParameters<ParametersDbContext>(optionsAction, redisSection);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection redisSection)
        where TDbContext : ParametersDbContext
    {
        services.AddScoped<IParameterManager, ParameterManager>();
        services.AddRedisDistributedCache(redisSection);
        return services.AddMakerChecker<TDbContext>(optionsAction);
    }
}
