using CodeNet.Elasticsearch.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Elasticsearch.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddElasticsearch(this WebApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.AddElasticsearch<ElasticsearchSettings>(sectionName);
        return webBuilder;
    }
    /// <summary>
    /// Add Elasticsearch Settings
    /// </summary>
    /// <typeparam name="TElasticsearchSettings">TElasticsearchSettings is ElasticsearchSettings</typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type ElasticsearchSettings</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddElasticsearch<TElasticsearchSettings>(this WebApplicationBuilder webBuilder, string sectionName) 
        where TElasticsearchSettings : ElasticsearchSettings
    {
        webBuilder.Services.Configure<TElasticsearchSettings>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }
}
