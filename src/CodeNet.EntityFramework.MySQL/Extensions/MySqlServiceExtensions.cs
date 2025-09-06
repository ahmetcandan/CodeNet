using CodeNet.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EntityFramework.MySQL.Extensions;

public static class MySqlServiceExtensions
{
    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static IServiceCollection AddMySQL(this IServiceCollection services, IConfiguration configuration, string connectionName)
         => services.AddMySQL<DbContext>(configuration, connectionName);

    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddMySQL<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionName)
        where TDbContext : DbContext
        => services.AddDbContext<TDbContext>(options => options.UseMySQL(configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(typeof(IConfiguration).Name, $"There is no '{connectionName}' in ConnectionStrings.")));

    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMySQL(this IServiceCollection services, string connectionString)
        => services.AddMySQL<DbContext>(connectionString);

    /// <summary>
    /// Add MySQL
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddMySQL<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
        => services.AddDbContext<TDbContext>(options => options.UseMySQL(connectionString));

    /// <summary>
    /// Use MySQL
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseMySQL(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        => MySQLDbContextOptionsExtensions.UseMySQL(optionsBuilder, connectionString);
}
