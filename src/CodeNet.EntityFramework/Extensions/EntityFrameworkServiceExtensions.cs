using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Extensions;

public static class EntityFrameworkServiceExtensions
{
    /// <summary>
    /// Add SqlServer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        return services.AddDbContext<DbContext>(configuration, connectionName);
    }

    /// <summary>
    /// Add SqlServer
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionName)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(typeof(IConfiguration).Name, $"There is no '{connectionName}' in ConnectionStrings.")));
    }

    /// <summary>
    /// Add SqlServer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<DbContext>(connectionString);
    }

    /// <summary>
    /// Add SqlServer
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDbContext<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
    }

    /// <summary>
    /// Use Sql Server
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        return optionsBuilder.UseSqlServer(connectionString);
    }
}
