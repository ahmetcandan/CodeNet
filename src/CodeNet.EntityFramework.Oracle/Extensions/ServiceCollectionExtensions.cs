using CodeNet.EntityFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Oracle.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddOracle(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddOracle<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddOracle<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseOracle(webBuilder.Configuration, connectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use Oracle
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder optionsBuilder, ConfigurationManager configuration, string connectionName)
    {
        return optionsBuilder.UseOracle(configuration.GetConnectionString(connectionName)!);
    }
}
