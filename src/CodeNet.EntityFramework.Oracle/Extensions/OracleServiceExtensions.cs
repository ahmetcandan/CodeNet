using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.Oracle.Extensions;

public static class OracleServiceExtensions
{
    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddOracle(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddOracle<DbContext>(connectionName);
    }

    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddOracle<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseOracle(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Use Oracle
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder optionsBuilder, IConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseOracle(configuration.GetConnectionString(connectionName)!);
    }

    /// <summary>
    /// Use Oracle
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseOracle<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, IConfigurationManager configuration, string connectionName)
        where TDbContext : DbContext
    {
        return optionsBuilder.UseOracle(configuration.GetConnectionString(connectionName)!);
    }
}
