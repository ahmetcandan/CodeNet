using CodeNet.EntityFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add InMemory DB
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddInMemoryDB(this WebApplicationBuilder webBuilder, string databaseName)
    {
        webBuilder.AddInMemoryDB(databaseName);
        return webBuilder;
    }

    /// <summary>    
    /// Add InMemory DB
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddInMemoryDB<TDbContext>(this WebApplicationBuilder webBuilder, string databaseName) 
        where TDbContext : DbContext
    {
        webBuilder.AddDbContext<TDbContext>(options => options.UseInMemoryDatabase(databaseName));
        return webBuilder;
    }

    /// <summary>
    /// Use InMemory DB
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseInMemoryDatabase(this DbContextOptionsBuilder optionsBuilder, string databaseName)
    {
        return InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilder, databaseName);
    }
}
