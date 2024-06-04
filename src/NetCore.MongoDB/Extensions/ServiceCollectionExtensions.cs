using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Abstraction.Model;

namespace NetCore.MongoDB.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMongoDB<TMongoDB, TModel, TMongoSettings>(this WebApplicationBuilder webBuilder, string sectionName) 
        where TMongoDB : BaseMongoRepository<TModel> 
        where TModel : INoSqlModel, new()
        where TMongoSettings : MongoDBSettings
    {
        webBuilder.Services.Configure<TMongoSettings>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => 
        {
            containerBuilder.RegisterType<TMongoDB>().As<TMongoDB>().InstancePerLifetimeScope();
        });
        return webBuilder;
    }
}
