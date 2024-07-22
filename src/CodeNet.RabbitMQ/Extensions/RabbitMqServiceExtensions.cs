﻿using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.RabbitMQ.Extensions;

public static class RabbitMqServiceExtensions
{
    /// <summary>
    /// Add RabbitMQ Consumer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer(this IServiceCollection services, IConfigurationSection rabbitSection)
    {
        return services.AddRabbitMQConsumer<RabbitMQConsumerService>(rabbitSection);
    }

    /// <summary>
    /// Add RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TConsumerService>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TConsumerService : RabbitMQConsumerService
    {
        _ = typeof(TConsumerService).Equals(typeof(RabbitMQConsumerService))
            ? services.Configure<RabbitMQConsumerOptions>(rabbitSection)
            : services.Configure<RabbitMQConsumerOptions<TConsumerService>>(rabbitSection);
        return services.AddScoped<TConsumerService, TConsumerService>();
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, IConfigurationSection rabbitSection)
    {
        return services.AddRabbitMQProducer<RabbitMQProducerService>(rabbitSection);
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer<TProducerService>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TProducerService : RabbitMQProducerService
    {
        _ = typeof(TProducerService).Equals(typeof(RabbitMQProducerService))
            ? services.Configure<RabbitMQProducerOptions>(rabbitSection)
            : services.Configure<RabbitMQProducerOptions<TProducerService>>(rabbitSection);
        return services.AddScoped<TProducerService, TProducerService>();
    }

    /// <summary>
    /// Use RabbitMQ Consumer
    /// IRabbitMQConsumerHandler<RabbitMQConsumerService> must be registered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQConsumer(this WebApplication app)
    {
        return app.UseRabbitMQConsumer<RabbitMQConsumerService>();
    }

    /// <summary>
    /// Use RabbitMQ Consumer
    /// IRabbitMQConsumerHandler<TConsumerService> must be registered.
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQConsumer<TConsumerService>(this WebApplication app)
        where TConsumerService : RabbitMQConsumerService
    {
        var serviceScope = app.Services.CreateScope();
        var consumerService = serviceScope.ServiceProvider.GetRequiredService<TConsumerService>();
        if (DependHandler(serviceScope, consumerService))
            app.Lifetime.ApplicationStarted.Register(consumerService.StartListening);

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <param name="listener"></param>
    private static bool DependHandler<TConsumerService>(IServiceScope serviceScope, TConsumerService listener)
        where TConsumerService : RabbitMQConsumerService
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IRabbitMQConsumerHandler<TConsumerService>>();
        if (messageHandlerService is not null)
        {
            listener.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
