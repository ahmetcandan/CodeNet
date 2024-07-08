using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.PostgreSQL.Extensions;

public static class PostgreSqlServiceExtensions
{
    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IServiceCollection AddNpgsql(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        return services.AddNpgsql<DbContext>(configuration, connectionName);
    }

    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddNpgsql<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionName)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(typeof(IConfiguration).Name, $"There is no '{connectionName}' in ConnectionStrings.")));
    }

    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddNpgsql(this IServiceCollection services, string connectionString)
    {
        return services.AddNpgsql<DbContext>(connectionString);
    }

    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddNpgsql<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString));
    }

    /// <summary>
    /// Use PostgeSQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseNpgsql(this DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        return NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(optionsBuilder, connectionString);
    }
}
