using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add RabbitMQ Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddRabbitMQ(this WebApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.AddRabbitMQ<RabbitMQSettings>(sectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add RabbitMQ Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddRabbitMQ<TRabbitMQSettings>(this WebApplicationBuilder webBuilder, string sectionName) where TRabbitMQSettings : RabbitMQSettings
    {
        webBuilder.Services.Configure<TRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }

    public static WebApplication UseRabbitMQConsumer<TData>(this WebApplication app)
        where TData : class, new()
    {
        var listener = app.Services.GetRequiredService<IRabbitMQConsumerService<TData>>(); 
        var messageHandlerServices = app.Services.GetServices<IRabbitMQConsumerHandler<TData>>();
        if (messageHandlerServices?.Any() == true)
        {
            foreach (var messageHandler in messageHandlerServices)
                listener.ReceivedMessage += messageHandler.Handler;

            app.Lifetime.ApplicationStarted.Register(listener.StartListening);
        }
        return app;
    }

    public static WebApplication UseRabbitMQConsumer<TRabbitMQConsumerService, TData>(this WebApplication app)
        where TData : class, new()
        where TRabbitMQConsumerService : IRabbitMQConsumerService<TData>
    {
        var listener = app.Services.GetRequiredService<TRabbitMQConsumerService>();
        var messageHandlerServices = app.Services.GetServices<IRabbitMQConsumerHandler<TData>>();
        if (messageHandlerServices?.Any() == true)
        {
            foreach (var messageHandler in messageHandlerServices)
                listener.ReceivedMessage += messageHandler.Handler;

            app.Lifetime.ApplicationStarted.Register(listener.StartListening);
        }
        return app;
    }
}
