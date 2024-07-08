using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.Extensions;

public static class EntityFrameworkServiceExtensions
{
    /// <summary>
    /// Add Sql Server
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDbContext(this IHostApplicationBuilder webBuilder, string connectionName)
    {
        return webBuilder.AddDbContext<DbContext>(connectionName);
    }

    /// <summary>    
    /// Add Sql Server
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDbContext<TDbContext>(this IHostApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseSqlServer(webBuilder.Configuration, connectionName));
    }

    /// <summary>
    /// Add DbContext Other Database
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDbContext<TDbContext>(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction)
        where TDbContext : DbContext
    {
        webBuilder.Services.AddDbContext<TDbContext>(optionsAction);
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
}
