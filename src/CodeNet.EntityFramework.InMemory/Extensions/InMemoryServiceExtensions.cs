using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.EntityFramework.InMemory.Extensions;

public static class InMemoryServiceExtensions
{
    /// <summary>
    /// Add InMemory DB
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddInMemoryDB(this IHostApplicationBuilder webBuilder, string databaseName)
    {
        return webBuilder.AddInMemoryDB(databaseName);
    }

    /// <summary>    
    /// Add InMemory DB
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddInMemoryDB<TDbContext>(this IHostApplicationBuilder webBuilder, string databaseName) 
        where TDbContext : DbContext
    {
        return webBuilder.AddDbContext<TDbContext>(options => options.UseInMemoryDatabase(databaseName));
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

    /// <summary>
    /// Use InMemory DB
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="optionsBuilder"></param>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder<TDbContext> UseInMemoryDatabase<TDbContext>(this DbContextOptionsBuilder<TDbContext> optionsBuilder, string databaseName)
        where TDbContext : DbContext
    {
        return InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilder, databaseName);
    }
}
