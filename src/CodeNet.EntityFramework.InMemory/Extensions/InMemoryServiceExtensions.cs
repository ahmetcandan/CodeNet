using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.InMemory.Extensions;

public static class InMemoryServiceExtensions
{
    /// <summary>
    /// Add InMemory DB
    /// </summary>
    /// <param name="services"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static IServiceCollection AddInMemoryDB(this IServiceCollection services, string databaseName) => services.AddInMemoryDB<DbContext>(databaseName);

    /// <summary>    
    /// Add InMemory DB
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="databaseName">Database Name</param>
    /// <returns></returns>
    public static IServiceCollection AddInMemoryDB<TDbContext>(this IServiceCollection services, string databaseName)
        where TDbContext : DbContext
        => services.AddDbContext<TDbContext>(options => options.UseInMemoryDatabase(databaseName));

    /// <summary>
    /// Use InMemory DB
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseInMemoryDatabase(this DbContextOptionsBuilder optionsBuilder, string databaseName)
         => InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilder, databaseName);
}
