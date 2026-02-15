using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.Oracle.Extensions;

public static class OracleServiceExtensions
{
    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IServiceCollection AddOracle(this IServiceCollection services, IConfiguration configuration, string connectionName)
        => services.AddOracle<DbContext>(configuration, connectionName);

    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddOracle<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionName)
        where TDbContext : DbContext
        => services.AddDbContext<TDbContext>(options => options.UseOracle(configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(typeof(IConfiguration).Name, $"There is no '{connectionName}' in ConnectionStrings.")));

    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddOracle(this IServiceCollection services, string connectionString)
        => services.AddOracle<DbContext>(connectionString);

    /// <summary>
    /// Add Oracle
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddOracle<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
        => services.AddDbContext<TDbContext>(options => options.UseOracle(connectionString));

    /// <summary>
    /// Use Oracle
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseOracle(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        => OracleDbContextOptionsExtensions.UseOracle(optionsBuilder, connectionString);
}
