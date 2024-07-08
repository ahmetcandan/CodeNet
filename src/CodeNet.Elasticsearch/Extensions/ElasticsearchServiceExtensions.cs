using CodeNet.Elasticsearch.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.Elasticsearch.Extensions;

public static class ElasticsearchServiceExtensions
{
    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddElasticsearch(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.AddElasticsearch<ElasticsearchSettings, ElasticsearchDbContext>(sectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <typeparam name="TElasticsearchSettings">TElasticsearchSettings is ElasticsearchSettings</typeparam>
    /// <typeparam name="TElasticsearchDbContext">TElasticsearchDbContext is ElasticsearchDbContext</typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type ElasticsearchSettings</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddElasticsearch<TElasticsearchSettings, TElasticsearchDbContext>(this IHostApplicationBuilder webBuilder, string sectionName) 
        where TElasticsearchSettings : ElasticsearchSettings
        where TElasticsearchDbContext : ElasticsearchDbContext
    {
        webBuilder.Services.Configure<TElasticsearchSettings>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddScoped<TElasticsearchDbContext, TElasticsearchDbContext>();
        return webBuilder;
    }
}
