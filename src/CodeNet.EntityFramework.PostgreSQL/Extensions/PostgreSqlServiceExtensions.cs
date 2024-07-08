using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.PostgreSQL.Extensions;

public static class PostgreSqlServiceExtensions
{
    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddNpgsql(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddNpgsql<DbContext>(connectionName);
    }

    /// <summary>
    /// Add PostgeSQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddNpgsql<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseNpgsql(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Use PostgeSQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseNpgsql(this DbContextOptionsBuilder optionsBuilder, IConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionName));
    }

    /// <summary>
    /// Use PostgeSQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseNpgsql<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, IConfigurationManager configuration, string connectionName)
        where TDbContext : DbContext
    {
        return optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionName));
    }
}
