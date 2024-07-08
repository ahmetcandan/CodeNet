using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.MongoDB.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mongoSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfigurationSection mongoSection)
    {
        return services.AddMongoDB<MongoDBSettings, MongoDBContext>(mongoSection);
    }

    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <typeparam name="TMongoSettings">TMongoSettings is MongoDBSettings</typeparam>
    /// <param name="services"></param>
    /// <param name="mongoSection">appSettings.json must contain the sectionName main block. Json must be type MongoDBSettings</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDB<TMongoSettings, TMongoDbContext>(this IServiceCollection services, IConfigurationSection mongoSection)
        where TMongoDbContext : MongoDBContext
        where TMongoSettings : MongoDBSettings
    {
        return services.Configure<TMongoSettings>(mongoSection)
            .AddScoped<TMongoDbContext, TMongoDbContext>();
    }
}
