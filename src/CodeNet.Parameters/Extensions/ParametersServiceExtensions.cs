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
    /// Add Parameters by EF Core
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection? parameterSection = null)
    {
        return services.AddParameters<ParametersDbContext>(optionsAction, parameterSection);
    }

    /// <summary>
    /// Add Parameters by EF Core
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection? parameterSection = null)
        where TDbContext : ParametersDbContext
    {
        if (parameterSection is not null)
            services.Configure<ParameterSettings>(parameterSection);
        else
            services.Configure<ParameterSettings>(c => { });

        services.AddScoped<IParameterManager, ParameterManager>();
        services.AddCodeNetContext();
        return services.AddMakerChecker<TDbContext>(optionsAction);
    }

    /// <summary>
    /// Parameters Use Redis
    /// </summary>
    /// <param name="services"></param>
    /// <param name="redisSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameterUseRedis(this IServiceCollection services, IConfigurationSection redisSection)
    {
        return services.AddRedisDistributedCache(redisSection);
    }
}
