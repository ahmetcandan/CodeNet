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
    {
        return services.AddRabbitMQConsumer<RabbitMQConsumerService, TConsumerHandler>(rabbitSection);
    }

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
    {
        return services.AddRabbitMQConsumer<RabbitMQConsumerService, TConsumerHandler>((RabbitMQConsumerOptions<RabbitMQConsumerService>)config);
    }

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
                c.QueueBind = config.QueueBind;
                c.Queue = config.Queue;
                c.DeclareQueue = config.DeclareQueue;
                c.Durable = config.Durable;
                c.AutoDelete = config.AutoDelete;
                c.Arguments = config.Arguments;
                c.QueueBindArguments = config.QueueBindArguments;
                c.ConnectionFactory = config.ConnectionFactory;
                c.DeclareExchange = config.DeclareExchange;
                c.Exchange = config.Exchange;
                c.RoutingKey = config.RoutingKey;
                c.ExchangeArguments = config.ExchangeArguments;
                c.ExchangeType = config.ExchangeType;
                c.Exclusive = config.Exclusive;
                c.AutoAck = config.AutoAck;
                c.AsyncConsumer = config.AsyncConsumer;
                c.NoLocal = config.NoLocal;
                c.ConsumerTag = config.ConsumerTag;
                c.Qos = config.Qos;
                c.ConsumerArguments = config.ConsumerArguments;
            })
            : services.Configure<RabbitMQConsumerOptions<TConsumerService>>(c =>
            {
                c.QueueBind = config.QueueBind;
                c.Queue = config.Queue;
                c.DeclareQueue = config.DeclareQueue;
                c.Durable = config.Durable;
                c.AutoDelete = config.AutoDelete;
                c.Arguments = config.Arguments;
                c.QueueBindArguments = config.QueueBindArguments;
                c.ConnectionFactory = config.ConnectionFactory;
                c.DeclareExchange = config.DeclareExchange;
                c.Exchange = config.Exchange;
                c.RoutingKey = config.RoutingKey;
                c.ExchangeArguments = config.ExchangeArguments;
                c.ExchangeType = config.ExchangeType;
                c.Exclusive = config.Exclusive;
                c.AutoAck = config.AutoAck;
                c.AsyncConsumer = config.AsyncConsumer;
                c.NoLocal = config.NoLocal;
                c.ConsumerTag = config.ConsumerTag;
                c.Qos = config.Qos;
                c.ConsumerArguments = config.ConsumerArguments;
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
    {
        return services.AddRabbitMQProducer((RabbitMQProducerOptions<RabbitMQProducerService>)config);
    }

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
                c.QueueBind = config.QueueBind;
                c.Queue = config.Queue;
                c.DeclareQueue = config.DeclareQueue;
                c.Durable = config.Durable;
                c.AutoDelete = config.AutoDelete;
                c.Arguments = config.Arguments;
                c.QueueBindArguments = config.QueueBindArguments;
                c.ConnectionFactory = config.ConnectionFactory;
                c.DeclareExchange = config.DeclareExchange;
                c.Exchange = config.Exchange;
                c.RoutingKey = config.RoutingKey;
                c.Mandatory = config.Mandatory;
                c.ExchangeArguments = config.ExchangeArguments;
                c.ExchangeType = config.ExchangeType;
                c.Exclusive = config.Exclusive;
            })
            : services.Configure<RabbitMQProducerOptions<TProducerService>>(c =>
            {
                c.QueueBind = config.QueueBind;
                c.Queue = config.Queue;
                c.DeclareQueue = config.DeclareQueue;
                c.Durable = config.Durable;
                c.AutoDelete = config.AutoDelete;
                c.Arguments = config.Arguments;
                c.QueueBindArguments = config.QueueBindArguments;
                c.ConnectionFactory = config.ConnectionFactory;
                c.DeclareExchange = config.DeclareExchange;
                c.Exchange = config.Exchange;
                c.RoutingKey = config.RoutingKey;
                c.Mandatory = config.Mandatory;
                c.ExchangeArguments = config.ExchangeArguments;
                c.ExchangeType = config.ExchangeType;
                c.Exclusive = config.Exclusive;
            });
        return services.AddScoped<TProducerService>();
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
