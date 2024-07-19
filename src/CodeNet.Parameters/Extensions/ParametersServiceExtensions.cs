using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using CodeNet.Redis.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.MakerChecker.Extensions;
using Microsoft.Extensions.Configuration;
using CodeNet.Parameters.Settings;
using CodeNet.Core.Extensions;

namespace CodeNet.Parameters.Extensions;

public static class ParametersServiceExtensions
{
    /// <summary>
    /// Add Parameters
    /// Use SqlServer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, string connectionString, IConfigurationSection redisSection, IConfigurationSection? parameterSection = null)
    {
        return services.AddParameters<ParametersDbContext>(connectionString, redisSection, parameterSection);
    }

    /// <summary>
    /// Add Parameters
    /// Use SqlServer
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionName"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters<TDbContext>(this IServiceCollection services, string connectionString, IConfigurationSection redisSection, IConfigurationSection? parameterSection = null)
        where TDbContext : ParametersDbContext
    {
        return services.AddParameters<TDbContext>(builder => builder.UseSqlServer(connectionString), redisSection, parameterSection);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection redisSection, IConfigurationSection? parameterSection = null)
    {
        return services.AddParameters<ParametersDbContext>(optionsAction, redisSection, parameterSection);
    }

    /// <summary>
    /// Add Parameters
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection redisSection, IConfigurationSection? parameterSection = null)
        where TDbContext : ParametersDbContext
    {
        if (parameterSection is not null)
            services.Configure<ParameterSettings>(parameterSection);
        else
            services.Configure<ParameterSettings>(c => { });

        services.AddScoped<IParameterManager, ParameterManager>();
        services.AddRedisDistributedCache(redisSection);
        services.AddCodeNetContext();
        return services.AddMakerChecker<TDbContext>(optionsAction);
    }
}
