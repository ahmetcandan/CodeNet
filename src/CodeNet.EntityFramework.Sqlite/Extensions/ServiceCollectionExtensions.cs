using CodeNet.EntityFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Sqlite.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSqlite(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddSqlite<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Sqlite
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSqlite<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseSqlite(webBuilder.Configuration, connectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use Sqlite
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlite(this DbContextOptionsBuilder optionsBuilder, ConfigurationManager configuration, string connectionName)
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
    public static DbContextOptionsBuilder UseSqlite<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, ConfigurationManager configuration, string connectionName)
        where TDbContext : DbContext
    {
        return optionsBuilder.UseSqlite(configuration.GetConnectionString(connectionName)!);
    }
}
