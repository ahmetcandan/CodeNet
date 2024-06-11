using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;

namespace NetCore.Abstraction.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add MongoDB BaseMongoRepository<TModel>
    /// </summary>
    /// <typeparam name="TMongoSettings">TMongoSettings is MongoDBSettings</typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type MongoDBSettings</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMongoDB<TMongoSettings>(this WebApplicationBuilder webBuilder, string sectionName) where TMongoSettings : MongoDBSettings
    {
        webBuilder.Services.Configure<TMongoSettings>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }
}
