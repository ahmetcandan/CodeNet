using CodeNet.StackExchange.Redis.Services;
using CodeNet.StackExchange.Redis.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.StackExchange.Redis.Extensions;

public static class StackExchangeServiceExtensions
{
    /// <summary>
    /// Add StackExchange Subscriber
    /// </summary>
    /// <typeparam name="TSubscribeHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeSubscribe<TSubscribeHandler>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TSubscribeHandler : class, IStackExchangeSubscribeHandler<StackExchangeSubscribeService>
    {
        return services.AddStackExcahangeSubscribe<StackExchangeSubscribeService, TSubscribeHandler>(stackExcahangeSection);
    }

    /// <summary>
    /// Add StackExchange Subscriber
    /// </summary>
    /// <typeparam name="TSubscribeService"></typeparam>
    /// <typeparam name="TSubscribeHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeSubscribe<TSubscribeService, TSubscribeHandler>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TSubscribeService : StackExchangeSubscribeService
        where TSubscribeHandler : class, IStackExchangeSubscribeHandler<TSubscribeService>
    {
        var options = stackExcahangeSection.Get<StackExchangePublisherOptions>() ?? throw new ArgumentNullException($"'{stackExcahangeSection.Path}' is null or empty in appSettings.json");
        return services.AddStackExcahangeSubscribe<TSubscribeService, TSubscribeHandler>(options);
    }

    /// <summary>
    /// Add StackExchange Subscriber
    /// </summary>
    /// <typeparam name="TSubscribeService"></typeparam>
    /// <typeparam name="TSubscribeHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangeSubscribe<TSubscribeService, TSubscribeHandler>(this IServiceCollection services, StackExchangeSubscribeOptions options)
        where TSubscribeService : StackExchangeSubscribeService
        where TSubscribeHandler : class, IStackExchangeSubscribeHandler<TSubscribeService>
    {
        _ = typeof(TSubscribeService).Equals(typeof(StackExchangeSubscribeService))
            ? services.Configure<StackExchangeSubscribeOptions>(c =>
            {
                c.Channel = options.Channel;
                c.Configuration = options.Configuration;
            })
            : services.Configure<StackExchangeSubscribeOptions<TSubscribeService>>(c =>
            {
                c.Channel = options.Channel;
                c.Configuration = options.Configuration;
            });

        services.AddSingleton<IStackExchangeSubscribeHandler<TSubscribeService>, TSubscribeHandler>();
        services.AddScoped<ISerializer, Serializer>();
        return services.AddSingleton<TSubscribeService>();
    }

    /// <summary>
    /// Add StackExchange Publisher
    /// </summary>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangePublisher(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
    {
        return services.AddStackExcahangePublisher<StackExchangePublisherService>(stackExcahangeSection);
    }

    /// <summary>
    /// Add StackExchange Publisher
    /// </summary>
    /// <typeparam name="TPublisherService"></typeparam>
    /// <param name="services"></param>
    /// <param name="stackExcahangeSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangePublisher<TPublisherService>(this IServiceCollection services, IConfigurationSection stackExcahangeSection)
        where TPublisherService : StackExchangePublisherService
    {
        var options = stackExcahangeSection.Get<StackExchangePublisherOptions>() ?? throw new ArgumentNullException($"'{stackExcahangeSection.Path}' is null or empty in appSettings.json");
        return services.AddStackExcahangePublisher<TPublisherService>(options);
    }

    /// <summary>
    /// Add StackExchange Publisher
    /// </summary>
    /// <typeparam name="TPublisherService"></typeparam>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddStackExcahangePublisher<TPublisherService>(this IServiceCollection services, StackExchangePublisherOptions options)
        where TPublisherService : StackExchangePublisherService
    {
        _ = typeof(TPublisherService).Equals(typeof(StackExchangePublisherService))
            ? services.Configure<StackExchangePublisherOptions>(c =>
            {
                c.Channel = options.Channel;
                c.CommandFlags = options.CommandFlags;
                c.Configuration = options.Configuration;
            })
            : services.Configure<StackExchangePublisherOptions<TPublisherService>>(c =>
            {
                c.Channel = options.Channel;
                c.CommandFlags = options.CommandFlags;
                c.Configuration = options.Configuration;
            });

        services.AddScoped<ISerializer, Serializer>();
        return services.AddScoped<TPublisherService>();
    }

    /// <summary>
    /// Use StackExchange Subscriber
    /// IStackExchangeSubscribeHandler<StackExchangeSubscribeService> must be registered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseStackExcahangeSubscriber(this WebApplication app)
    {
        return app.UseStackExcahangeSubscriber<StackExchangeSubscribeService>();
    }

    /// <summary>
    /// Use StackExchange Subscriber
    /// IStackExchangeSubscribeHandler<TSubscribeService> must be registered.
    /// </summary>
    /// <typeparam name="TSubscribeService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseStackExcahangeSubscriber<TSubscribeService>(this WebApplication app)
        where TSubscribeService : StackExchangeSubscribeService
    {
        var serviceScope = app.Services.CreateScope();
        var subscribeService = serviceScope.ServiceProvider.GetService<TSubscribeService>() ?? throw new NotImplementedException($"'{nameof(TSubscribeService)}' not implemented service.");
        if (DependHandler(serviceScope, subscribeService))
            app.Lifetime.ApplicationStarted.Register(async () => await subscribeService.StartListening());

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TSubscribeService"></typeparam>
    /// <param name="app"></param>
    /// <param name="subscribeService"></param>
    private static bool DependHandler<TSubscribeService>(IServiceScope serviceScope, TSubscribeService subscribeService)
        where TSubscribeService : StackExchangeSubscribeService
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IStackExchangeSubscribeHandler<TSubscribeService>>();
        if (messageHandlerService is not null)
        {
            subscribeService.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
