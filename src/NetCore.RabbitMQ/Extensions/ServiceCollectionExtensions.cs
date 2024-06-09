using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;

namespace NetCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add RabbitMQ
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddRabbitMQ<TRabbitMQSettings>(this WebApplicationBuilder webBuilder, string sectionName) where TRabbitMQSettings : RabbitMQSettings
    {
        webBuilder.Services.Configure<TRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }
}
