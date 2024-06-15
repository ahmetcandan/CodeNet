using CodeNet.EntityFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.MySQL.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMySQL(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddMySQL<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMySQL<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseMySQL(webBuilder.Configuration, connectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use MySQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseMySQL(this DbContextOptionsBuilder optionsBuilder, ConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseMySQL(configuration.GetConnectionString(connectionName)!);
    }
}
