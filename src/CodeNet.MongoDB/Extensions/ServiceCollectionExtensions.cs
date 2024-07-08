using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.MongoDB.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMongoDB(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        return webBuilder.AddMongoDB<MongoDBSettings, MongoDBContext>(sectionName);
    }

    /// <summary>
    /// Add MongoDB Settings
    /// </summary>
    /// <typeparam name="TMongoSettings">TMongoSettings is MongoDBSettings</typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type MongoDBSettings</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMongoDB<TMongoSettings, TMongoDbContext>(this IHostApplicationBuilder webBuilder, string sectionName)
        where TMongoDbContext : MongoDBContext
        where TMongoSettings : MongoDBSettings
    {
        webBuilder.Services.Configure<TMongoSettings>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddScoped<TMongoDbContext, TMongoDbContext>();
        return webBuilder;
    }
}
