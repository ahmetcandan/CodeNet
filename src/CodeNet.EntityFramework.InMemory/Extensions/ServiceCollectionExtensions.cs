using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Extensions;

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
        webBuilder.Services.AddDbContext<TDbContext>(options => options.UseInMemoryDatabase(databaseName));
        return webBuilder;
    }
}
