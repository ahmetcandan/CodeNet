using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.Abstraction;
using CodeNet.Abstraction.Model;

namespace CodeNet.Extensions;

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
    public static WebApplicationBuilder AddRabbitMQ<TRabbitMQSettings>(this WebApplicationBuilder webBuilder, string sectionName) 
        where TRabbitMQSettings : RabbitMQSettings
    {
        webBuilder.Services.Configure<TRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQConsumer<TData>(this WebApplication app)
        where TData : class, new()
    {
        var listener = app.Services.GetRequiredService<IRabbitMQConsumerService<TData>>();
        DependHandler(app, listener);
        return app;
    }

    /// <summary>
    /// Use RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TRabbitMQConsumerService"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQConsumer<TRabbitMQConsumerService, TData>(this WebApplication app)
        where TData : class, new()
        where TRabbitMQConsumerService : IRabbitMQConsumerService<TData>
    {
        var listener = app.Services.GetRequiredService<TRabbitMQConsumerService>();
        DependHandler(app, listener);
        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="app"></param>
    /// <param name="listener"></param>
    private static void DependHandler<TData>(WebApplication app, IRabbitMQConsumerService<TData> listener)
        where TData : class, new()
    {
        var messageHandlerServices = app.Services.GetServices<IRabbitMQConsumerHandler<TData>>();
        if (messageHandlerServices?.Any() == true)
        {
            foreach (var messageHandler in messageHandlerServices)
                listener.ReceivedMessage += messageHandler.Handler;

            app.Lifetime.ApplicationStarted.Register(listener.StartListening);
        }
    }
}
