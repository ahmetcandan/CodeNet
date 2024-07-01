using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Sql Server
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddDbContext<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>    
    /// Add Sql Server
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDbContext<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseSqlServer(webBuilder.Configuration, connectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use Sql Server
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration, string connectionName)
    {
        return optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionName));
    }

    /// <summary>
    /// Add DbContext Other Database
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDbContext<TDbContext>(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : DbContext
    {
        webBuilder.Services.AddDbContext<TDbContext>(optionsAction);
        return webBuilder;
    }
}
