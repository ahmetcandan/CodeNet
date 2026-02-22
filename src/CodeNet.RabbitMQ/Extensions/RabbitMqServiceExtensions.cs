using CodeNet.RabbitMQ.Services;
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
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TConsumerHandler>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TConsumerHandler : class, IRabbitMQConsumerHandler<RabbitMQConsumerService>
        => services.AddRabbitMQConsumer<RabbitMQConsumerService, TConsumerHandler>(rabbitSection);

    /// <summary>
    /// Add RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TConsumerService, TConsumerHandler>(this IServiceCollection services, IConfigurationSection rabbitSection)
        where TConsumerService : RabbitMQConsumerService
        where TConsumerHandler : class, IRabbitMQConsumerHandler<TConsumerService>
    {
        var options = rabbitSection.Get<RabbitMQConsumerOptions<TConsumerService>>() ?? throw new ArgumentNullException($"'{rabbitSection.Path}' is null or empty in appSettings.json");
        return services.AddRabbitMQConsumer<TConsumerService, TConsumerHandler>(options);
    }

    /// <summary>
    /// Add RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TConsumerHandler>(this IServiceCollection services, RabbitMQConsumerOptions config)
        where TConsumerHandler : class, IRabbitMQConsumerHandler<RabbitMQConsumerService>
        => services.AddRabbitMQConsumer<RabbitMQConsumerService, TConsumerHandler>((RabbitMQConsumerOptions<RabbitMQConsumerService>)config);

    /// <summary>
    /// Add RabbitMQ Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQConsumer<TConsumerService, TConsumerHandler>(this IServiceCollection services, RabbitMQConsumerOptions<TConsumerService> config)
        where TConsumerService : RabbitMQConsumerService
        where TConsumerHandler : class, IRabbitMQConsumerHandler<TConsumerService>
    {
        _ = typeof(TConsumerService).Equals(typeof(RabbitMQConsumerService))
            ? services.Configure<RabbitMQConsumerOptions>(c =>
            {
                ConfigurationCopy(c, config);
            })
            : services.Configure<RabbitMQConsumerOptions<TConsumerService>>(c =>
            {
                ConfigurationCopy(c, config);
            });

        services.AddSingleton<IRabbitMQConsumerHandler<TConsumerService>, TConsumerHandler>();
        return services.AddSingleton<TConsumerService>();
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rabbitSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, IConfigurationSection rabbitSection)
        => services.AddRabbitMQProducer<RabbitMQProducerService>(rabbitSection);

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
        var options = rabbitSection.Get<RabbitMQProducerOptions<TProducerService>>() ?? throw new ArgumentNullException($"'{rabbitSection.Path}' is null or empty in appSettings.json");
        return AddRabbitMQProducer(services, options);
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, RabbitMQProducerOptions config)
        => services.AddRabbitMQProducer((RabbitMQProducerOptions<RabbitMQProducerService>)config);

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQProducer<TProducerService>(this IServiceCollection services, RabbitMQProducerOptions<TProducerService> config)
    where TProducerService : RabbitMQProducerService
    {
        _ = typeof(TProducerService).Equals(typeof(RabbitMQProducerService))
            ? services.Configure<RabbitMQProducerOptions>(c =>
            {
                ConfigurationCopy(c, config);
            })
            : services.Configure<RabbitMQProducerOptions<TProducerService>>(c =>
            {
                ConfigurationCopy(c, config);
            });
        return services.AddScoped<TProducerService>();
    }

    private static void ConfigurationCopy(RabbitMQConsumerOptions currentOptions, RabbitMQConsumerOptions baseOptions)
    {
        BaseConfigurationCopy(currentOptions, baseOptions);
        currentOptions.AutoAck = baseOptions.AutoAck;
        currentOptions.AsyncConsumer = baseOptions.AsyncConsumer;
        currentOptions.NoLocal = baseOptions.NoLocal;
        currentOptions.ConsumerTag = baseOptions.ConsumerTag;
        currentOptions.Qos = baseOptions.Qos;
        currentOptions.ConsumerArguments = baseOptions.ConsumerArguments;
    }

    private static void ConfigurationCopy(RabbitMQProducerOptions currentOptions, RabbitMQProducerOptions baseOptions)
    {
        BaseConfigurationCopy(currentOptions, baseOptions);
        currentOptions.Mandatory = baseOptions.Mandatory;
    }

    private static void BaseConfigurationCopy(BaseRabbitMQOptions currentOptions, BaseRabbitMQOptions baseOptions)
    {
        currentOptions.QueueBind = baseOptions.QueueBind;
        currentOptions.Queue = baseOptions.Queue;
        currentOptions.DeclareQueue = baseOptions.DeclareQueue;
        currentOptions.Durable = baseOptions.Durable;
        currentOptions.AutoDelete = baseOptions.AutoDelete;
        currentOptions.Arguments = baseOptions.Arguments;
        currentOptions.QueueBindArguments = baseOptions.QueueBindArguments;
        currentOptions.ConnectionFactory = baseOptions.ConnectionFactory;
        currentOptions.DeclareExchange = baseOptions.DeclareExchange;
        currentOptions.Exchange = baseOptions.Exchange;
        currentOptions.RoutingKey = baseOptions.RoutingKey;
        currentOptions.ExchangeArguments = baseOptions.ExchangeArguments;
        currentOptions.ExchangeType = baseOptions.ExchangeType;
        currentOptions.Exclusive = baseOptions.Exclusive;
    }

    /// <summary>
    /// Use RabbitMQ Consumer
    /// IRabbitMQConsumerHandler<RabbitMQConsumerService> must be registered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQConsumer(this WebApplication app) => app.UseRabbitMQConsumer<RabbitMQConsumerService>();

    /// <summary>
    /// Use RabbitMQ Consumer
    /// IRabbitMQConsumerHandler<TConsumerService> must be registered.
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseRabbitMQConsumer<TConsumerService>(this WebApplication app)
        where TConsumerService : RabbitMQConsumerService
    {
        var serviceScope = app.Services.CreateScope();
        var consumerService = serviceScope.ServiceProvider.GetService<TConsumerService>() ?? throw new NotImplementedException($"'{nameof(TConsumerService)}' not implemented service.");
        if (DependHandler(serviceScope, consumerService))
            app.Lifetime.ApplicationStarted.Register(consumerService.StartListening);

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <param name="consumerService"></param>
    private static bool DependHandler<TConsumerService>(IServiceScope serviceScope, TConsumerService consumerService)
        where TConsumerService : RabbitMQConsumerService
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IRabbitMQConsumerHandler<TConsumerService>>();
        if (messageHandlerService is not null)
        {
            consumerService.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
