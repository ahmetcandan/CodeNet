using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.MySQL.Extensions;

public static class MySqlServiceExtensions
{
    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMySQL(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddMySQL<DbContext>(connectionName);
    }

    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMySQL<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseMySQL(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Use MySQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseMySQL(this DbContextOptionsBuilder optionsBuilder, IConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseMySQL(configuration.GetConnectionString(connectionName)!);
    }

    /// <summary>
    /// Use MySQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseMySQL<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, IConfigurationManager configuration, string connectionName)
        where TDbContext : DbContext
    {
        return optionsBuilder.UseMySQL(configuration.GetConnectionString(connectionName)!);
    }
}
