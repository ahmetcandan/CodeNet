using CodeNet.MakerChecker.Extensions;
using CodeNet.Parameters.DbContext;
using CodeNet.Redis.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Parameters.Extensions;

public class ParameterOptionsBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get { return services; } }

    public ParameterOptionsBuilder AddDbContext(Action<DbContextOptionsBuilder> action) => AddDbContext<ParametersDbContext>(action);

    public ParameterOptionsBuilder AddDbContext<TDbContext>(Action<DbContextOptionsBuilder> action)
        where TDbContext : ParametersDbContext
    {
        Services.AddMakerChecker<TDbContext>(action);
        return this;
    }

    /// <summary>
    /// Add Redis
    /// </summary>
    /// <param name="redisSection"></param>
    /// <returns></returns>
    public ParameterOptionsBuilder AddRedis(IConfigurationSection redisSection)
    {
        Services.AddRedisDistributedCache(redisSection);
        return this;
    }
}
