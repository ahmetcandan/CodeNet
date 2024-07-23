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
        _ = typeof(TMongoDbContext).Equals(typeof(MongoDBContext))
            ? services.Configure<MongoDbOptions>(mongoSection)
            : services.Configure<MongoDbOptions<TMongoDbContext>>(mongoSection);
        return services.AddScoped<TMongoDbContext>();
    }
}
