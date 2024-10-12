using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.MongoDB.Extensions;

public static class MongoDBServiceCollectionExtensions
{
    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mongoSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfigurationSection mongoSection)
    {
        return services.AddMongoDB<MongoDBContext>(mongoSection);
    }

    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <typeparam name="TMongoOptions">TMongoSettings is MongoDbOptions</typeparam>
    /// <param name="services"></param>
    /// <param name="mongoSection">appSettings.json must contain the sectionName main block. Json must be type MongoDbOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDB<TMongoDbContext>(this IServiceCollection services, IConfigurationSection mongoSection)
        where TMongoDbContext : MongoDBContext
    {
        var options = mongoSection.Get<MongoDbOptions<TMongoDbContext>>() ?? throw new ArgumentNullException($"'{mongoSection.Path}' is null or empty in appSettings.json");
        return AddMongoDB(services, options);
    }

    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <typeparam name="TMongoDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="mongoDbOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDB<TMongoDbContext>(this IServiceCollection services, MongoDbOptions<TMongoDbContext> mongoDbOptions)
    where TMongoDbContext : MongoDBContext
    {
        _ = typeof(TMongoDbContext).Equals(typeof(MongoDBContext))
            ? services.Configure<MongoDbOptions>(c =>
            {
                c.ConnectionString = mongoDbOptions.ConnectionString;
                c.DatabaseName = mongoDbOptions.DatabaseName;
            })
            : services.Configure<MongoDbOptions<TMongoDbContext>>(c =>
            {
                c.ConnectionString = mongoDbOptions.ConnectionString;
                c.DatabaseName = mongoDbOptions.DatabaseName;
            });
        return services.AddScoped<TMongoDbContext>();
    }
}
