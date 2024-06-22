using CodeNet.Logging.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace CodeNet.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Logging
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">LoggingSettings section</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder webBuilder, string sectionName)
    {
        var loggingSettings = webBuilder.Configuration.GetSection(sectionName).Get<LoggingSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(loggingSettings.ElasticsearchUrl))
        {
            AutoRegisterTemplate = loggingSettings.AutoRegisterTemplate,
            IndexFormat = loggingSettings.IndexFormat,
            ModifyConnectionSettings = configuration => configuration.BasicAuthentication(loggingSettings.Username, loggingSettings.Password)
        })
        .CreateLogger();
        return webBuilder;
    }

    /// <summary>
    /// Use Logging
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
}
