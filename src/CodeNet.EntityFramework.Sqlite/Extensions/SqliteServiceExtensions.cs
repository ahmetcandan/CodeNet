using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.Sqlite.Extensions;

public static class SqliteServiceExtensions
{
    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddSqlite(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddSqlite<DbContext>(connectionName);
    }

    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddSqlite<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseSqlite(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Use Sqlite
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder optionsBuilder, IConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseSqlite(configuration.GetConnectionString(connectionName)!);
    }

    /// <summary>
    /// Use Sqlite
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlite<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, IConfigurationManager configuration, string connectionName)
        where TDbContext : DbContext
    {
        return optionsBuilder.UseSqlite(configuration.GetConnectionString(connectionName)!);
    }
}
