using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.RabbitMQ.Extensions;

public static class RabbitMqServiceExtensions
{
    /// <summary>
    /// Add RabbitMQ Consumer Settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer(this IServiceCollection services, IConfigurationSection rabbitSection)
    {
        return services.AddRabbitMQConsumer<RabbitMQConsumerSettings>(rabbitSection);
    }

    /// <summary>
    /// Add RabbitMQ Consumer Settings
    /// </summary>
    /// <typeparam name="TRabbitMQSettings"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TRabbitMQSettings>(this IServiceCollection services, IConfigurationSection rabbitSection) 
        where TRabbitMQSettings : RabbitMQConsumerSettings
    {
        services.Configure<TRabbitMQSettings>(rabbitSection);
        return services.AddScoped(typeof(IRabbitMQConsumerService<>), typeof(RabbitMQConsumerService<>));
    }

    /// <summary>
    /// Add RabbitMQ Producer Settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, IConfigurationSection rabbitSection)
    {
        return services.AddRabbitMQProducer<RabbitMQProducerSettings>(rabbitSection);
    }

    /// <summary>
    /// Add RabbitMQ Producer Settings
    /// </summary>
    /// <typeparam name="TRabbitMQSettings"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer<TRabbitMQSettings>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TRabbitMQSettings : RabbitMQProducerSettings
    {
        services.Configure<TRabbitMQSettings>(rabbitSection);
        return services.AddScoped(typeof(IRabbitMQProducerService<>), typeof(RabbitMQProducerService<>));
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
