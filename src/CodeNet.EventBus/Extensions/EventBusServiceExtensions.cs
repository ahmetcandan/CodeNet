using CodeNet.EventBus.Services;
using CodeNet.EventBus.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.EventBus.Extensions;

public static class EventBusServiceExtensions
{
    /// <summary>
    /// Add EventBus Subscriber
    /// </summary>
    /// <typeparam name="TSubscriberHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusMQSubscriber<TSubscriberHandler>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TSubscriberHandler : class, IEventBusSubscriberHandler<EventBusSubscriberService>
    {
        return services.AddEventBusMQSubscriber<EventBusSubscriberService, TSubscriberHandler>(rabbitSection);
    }

    /// <summary>
    /// Add EventBus Subscriber
    /// </summary>
    /// <typeparam name="TSubscriberService"></typeparam>
    /// <typeparam name="TSubscriberHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddEventBusMQSubscriber<TSubscriberService, TSubscriberHandler>(this IServiceCollection services, IConfigurationSection section)
        where TSubscriberService : EventBusSubscriberService
        where TSubscriberHandler : class, IEventBusSubscriberHandler<TSubscriberService>
    {
        var options = section.Get<EventBusSubscriberOptions<TSubscriberService>>() ?? throw new ArgumentNullException($"'{section.Path}' is null or empty in appSettings.json");
        return services.AddEventBusMQSubscriber<TSubscriberService, TSubscriberHandler>(options);
    }

    /// <summary>
    /// Add EventBus Subscriber
    /// </summary>
    /// <typeparam name="TSubscriberHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusMQSubscriber<TSubscriberHandler>(this IServiceCollection services, EventBusSubscriberOptions config)
        where TSubscriberHandler : class, IEventBusSubscriberHandler<EventBusSubscriberService>
    {
        return services.AddEventBusMQSubscriber<EventBusSubscriberService, TSubscriberHandler>((EventBusSubscriberOptions<EventBusSubscriberService>)config);
    }

    /// <summary>
    /// Add EventBus Subscriber
    /// </summary>
    /// <typeparam name="TSubscriberService"></typeparam>
    /// <typeparam name="TSubscriberHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusMQSubscriber<TSubscriberService, TSubscriberHandler>(this IServiceCollection services, EventBusSubscriberOptions<TSubscriberService> config)
        where TSubscriberService : EventBusSubscriberService
        where TSubscriberHandler : class, IEventBusSubscriberHandler<TSubscriberService>
    {
        _ = typeof(TSubscriberService).Equals(typeof(EventBusSubscriberService))
            ? services.Configure<EventBusSubscriberOptions>(c =>
            {
                c.HostName = config.HostName;
                c.Port = config.Port;
                c.Channel = config.Channel;
                c.ConsumerGroup = config.ConsumerGroup;
            })
            : services.Configure<EventBusSubscriberOptions<TSubscriberService>>(c =>
            {
                c.HostName = config.HostName;
                c.Port = config.Port;
                c.Channel = config.Channel;
                c.ConsumerGroup = config.ConsumerGroup;
            });

        services.AddSingleton<IEventBusSubscriberHandler<TSubscriberService>, TSubscriberHandler>();
        return services.AddSingleton<TSubscriberService>();
    }

    /// <summary>
    /// Add EventBus Publisher
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusPublisher(this IServiceCollection services, IConfigurationSection rabbitSection)
    {
        return services.AddEventBusPublisher<EventBusPublisherService>(rabbitSection);
    }

    /// <summary>
    /// Add EventBus Publisher
    /// </summary>
    /// <typeparam name="TPublisherService"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddEventBusPublisher<TPublisherService>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TPublisherService : EventBusPublisherService
    {
        var options = rabbitSection.Get<EventBusPublisherOptions<TPublisherService>>() ?? throw new ArgumentNullException($"'{rabbitSection.Path}' is null or empty in appSettings.json");
        return AddEventBusPublisher(services, options);
    }

    /// <summary>
    /// Add EventBus Publisher
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusPublisher(this IServiceCollection services, EventBusPublisherOptions config)
    {
        return services.AddEventBusPublisher((EventBusPublisherOptions<EventBusPublisherService>)config);
    }

    /// <summary>
    /// Add EventBus Publisher
    /// </summary>
    /// <typeparam name="TPublisherService"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusPublisher<TPublisherService>(this IServiceCollection services, EventBusPublisherOptions<TPublisherService> config)
    where TPublisherService : EventBusPublisherService
    {
        _ = typeof(TPublisherService).Equals(typeof(EventBusPublisherService))
            ? services.Configure<EventBusPublisherOptions>(c =>
            {
                c.HostName = config.HostName;
                c.Port = config.Port;
                c.Channel = config.Channel;
            })
            : services.Configure<EventBusPublisherOptions<TPublisherService>>(c =>
            {
                c.HostName = config.HostName;
                c.Port = config.Port;
                c.Channel = config.Channel;
            });
        return services.AddScoped<TPublisherService>();
    }

    /// <summary>
    /// Use EventBus Subscriber
    /// IEventBusSubscriberHandler<EventBusSubscriberService> must be registered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseEventBusSubscriber(this WebApplication app)
    {
        return app.UseEventBusSubscriber<EventBusSubscriberService>();
    }

    /// <summary>
    /// Use EventBus Subscriber
    /// IEventBusSubscriberHandler<TSubscriberService> must be registered.
    /// </summary>
    /// <typeparam name="TSubscriberService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseEventBusSubscriber<TSubscriberService>(this WebApplication app)
        where TSubscriberService : EventBusSubscriberService
    {
        var serviceScope = app.Services.CreateScope();
        var subscriberService = serviceScope.ServiceProvider.GetService<TSubscriberService>() ?? throw new NotImplementedException($"'{nameof(TSubscriberService)}' not implemented service.");
        if (DependHandler(serviceScope, subscriberService))
            app.Lifetime.ApplicationStarted.Register(subscriberService.StartListening);

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TSubscriberService"></typeparam>
    /// <param name="serviceScope"></param>
    /// <param name="subscriberService"></param>
    /// <returns></returns>
    private static bool DependHandler<TSubscriberService>(IServiceScope serviceScope, TSubscriberService subscriberService)
        where TSubscriberService : EventBusSubscriberService
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IEventBusSubscriberHandler<TSubscriberService>>();
        if (messageHandlerService is not null)
        {
            subscriberService.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
