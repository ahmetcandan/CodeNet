using CodeNet.Elasticsearch.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Elasticsearch.Extensions;

public static class ElasticsearchServiceExtensions
{
    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        return services.AddElasticsearch<ElasticsearchSettings, ElasticsearchDbContext>(configurationSection);
    }

    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <typeparam name="TElasticsearchSettings"></typeparam>
    /// <typeparam name="TElasticsearchDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddElasticsearch<TElasticsearchSettings, TElasticsearchDbContext>(this IServiceCollection services, IConfigurationSection configurationSection) 
        where TElasticsearchSettings : ElasticsearchSettings
        where TElasticsearchDbContext : ElasticsearchDbContext
    {
        services.Configure<TElasticsearchSettings>(configurationSection);
        return services.AddScoped<TElasticsearchDbContext, TElasticsearchDbContext>();
    }
}
