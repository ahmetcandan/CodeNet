using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.RabbitMQ.Extensions;

public static class RabbitMqServiceExtensions
{
    /// <summary>
    /// Add RabbitMQ Consumer Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddRabbitMQConsumer(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        return webBuilder.AddRabbitMQConsumer<RabbitMQConsumerSettings>(sectionName);
    }

    /// <summary>
    /// Add RabbitMQ Consumer Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddRabbitMQConsumer<TRabbitMQSettings>(this IHostApplicationBuilder webBuilder, string sectionName) 
        where TRabbitMQSettings : RabbitMQConsumerSettings
    {
        webBuilder.Services.Configure<TRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddScoped(typeof(IRabbitMQConsumerService<>), typeof(RabbitMQConsumerService<>));
        return webBuilder;
    }

    /// <summary>
    /// Add RabbitMQ Producer Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddRabbitMQProducer(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        return webBuilder.AddRabbitMQProducer<RabbitMQProducerSettings>(sectionName);
    }

    /// <summary>
    /// Add RabbitMQ Producer Settings
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddRabbitMQProducer<TRabbitMQSettings>(this IHostApplicationBuilder webBuilder, string sectionName)
        where TRabbitMQSettings : RabbitMQProducerSettings
    {
        webBuilder.Services.Configure<TRabbitMQSettings>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddScoped(typeof(IRabbitMQProducerService<>), typeof(RabbitMQProducerService<>));
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
