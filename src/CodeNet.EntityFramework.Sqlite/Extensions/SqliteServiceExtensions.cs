using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Sqlite.Extensions;

public static class SqliteServiceExtensions
{
    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IServiceCollection AddSqlite(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        return services.AddSqlite<DbContext>(configuration, connectionName);
    }

    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSqlite<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionName)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseSqlite(configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(typeof(IConfiguration).Name, $"There is no '{connectionName}' in ConnectionStrings.")));
    }

    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlite(this IServiceCollection services, string connectionString)
    {
        return services.AddSqlite<DbContext>(connectionString);
    }

    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlite<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        return services.AddDbContext<TDbContext>(options => options.UseSqlite(connectionString));
    }

    /// <summary>
    /// Use Sqlite
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        return SqliteDbContextOptionsBuilderExtensions.UseSqlite(optionsBuilder, connectionString);
    }
}
