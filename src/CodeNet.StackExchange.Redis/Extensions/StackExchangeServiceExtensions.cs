﻿using CodeNet.StackExchange.Redis.Services;
using CodeNet.StackExchange.Redis.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.StackExchange.Redis.Extensions;

public static class StackExchangeServiceExtensions
{
    /// <summary>
    /// Add StackExchange Consumer
    /// </summary>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeConsumer<TConsumerHandler>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TConsumerHandler : class, IStackExchangeConsumerHandler<StackExchangeConsumerService>
    {
        return services.AddStackExcahangeConsumer<StackExchangeConsumerService, TConsumerHandler>(stackExcahangeSection);
    }

    /// <summary>
    /// Add StackExchange Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeConsumer<TConsumerService, TConsumerHandler>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TConsumerService : StackExchangeConsumerService
        where TConsumerHandler : class, IStackExchangeConsumerHandler<TConsumerService>
    {
        var options = stackExcahangeSection.Get<StackExchangeProducerOptions>() ?? throw new ArgumentNullException($"'{stackExcahangeSection.Path}' is null or empty in appSettings.json");
        return services.AddStackExcahangeConsumer<TConsumerService, TConsumerHandler>(options);
    }

    /// <summary>
    /// Add StackExchange Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeConsumer<TConsumerService, TConsumerHandler>(this IServiceCollection services, StackExchangeConsumerOptions options)
        where TConsumerService : StackExchangeConsumerService
        where TConsumerHandler : class, IStackExchangeConsumerHandler<TConsumerService>
    {
        _ = typeof(TConsumerService).Equals(typeof(StackExchangeConsumerService))
            ? services.Configure<StackExchangeConsumerOptions>(c =>
            {
                c.Channel = options.Channel;
                c.Configuration = options.Configuration;
            })
            : services.Configure<StackExchangeConsumerOptions<TConsumerService>>(c =>
            {
                c.Channel = options.Channel;
                c.Configuration = options.Configuration;
            });

        services.AddSingleton<IStackExchangeConsumerHandler<TConsumerService>, TConsumerHandler>();
        services.AddScoped<ISerializer, Serializer>();
        return services.AddSingleton<TConsumerService>();
    }

    /// <summary>
    /// Add StackExchange Producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeProducer(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
    {
        return services.AddStackExcahangeProducer<StackExchangeProducerService>(stackExcahangeSection);
    }

    /// <summary>
    /// Add StackExchange Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeProducer<TProducerService>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TProducerService : StackExchangeProducerService
    {
        var options = stackExcahangeSection.Get<StackExchangeProducerOptions>() ?? throw new ArgumentNullException($"'{stackExcahangeSection.Path}' is null or empty in appSettings.json");
        return services.AddStackExcahangeProducer<TProducerService>(options);
    }

    /// <summary>
    /// Add StackExchange Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeProducer<TProducerService>(this IServiceCollection services, StackExchangeProducerOptions options)
        where TProducerService : StackExchangeProducerService
    {
        _ = typeof(TProducerService).Equals(typeof(StackExchangeProducerService))
            ? services.Configure<StackExchangeProducerOptions>(c =>
            {
                c.Channel = options.Channel;
                c.CommandFlags = options.CommandFlags;
                c.Configuration = options.Configuration;
            })
            : services.Configure<StackExchangeProducerOptions<TProducerService>>(c =>
            {
                c.Channel = options.Channel;
                c.CommandFlags = options.CommandFlags;
                c.Configuration = options.Configuration;
            });

        services.AddScoped<ISerializer, Serializer>();
        return services.AddScoped<TProducerService>();
    }

    /// <summary>
    /// Use StackExchange Consumer
    /// IStackExchangeConsumerHandler<StackExchangeConsumerService> must be registered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseStackExcahangeConsumer(this WebApplication app)
    {
        return app.UseStackExcahangeConsumer<StackExchangeConsumerService>();
    }

    /// <summary>
    /// Use StackExcahange Consumer
    /// IStackExchangeConsumerHandler<TConsumerService> must be registered.
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseStackExcahangeConsumer<TConsumerService>(this WebApplication app)
        where TConsumerService : StackExchangeConsumerService
    {
        var serviceScope = app.Services.CreateScope();
        var consumerService = serviceScope.ServiceProvider.GetService<TConsumerService>() ?? throw new NotImplementedException($"'{nameof(TConsumerService)}' not implemented service.");
        if (DependHandler(serviceScope, consumerService))
            app.Lifetime.ApplicationStarted.Register(async () => await consumerService.StartListening());

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <param name="consumerService"></param>
    private static bool DependHandler<TConsumerService>(IServiceScope serviceScope, TConsumerService consumerService)
        where TConsumerService : StackExchangeConsumerService
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IStackExchangeConsumerHandler<TConsumerService>>();
        if (messageHandlerService is not null)
        {
            consumerService.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
