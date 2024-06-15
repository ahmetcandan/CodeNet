using CodeNet.EntityFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CodeNet.EntityFramework.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddNpgsql(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddNpgsql<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddNpgsql<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseNpgsql(webBuilder.Configuration, connectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use PostgeSQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseNpgsql(this DbContextOptionsBuilder optionsBuilder, ConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionName));
    }
}
