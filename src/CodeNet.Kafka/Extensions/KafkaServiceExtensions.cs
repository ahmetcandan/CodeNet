using CodeNet.Kafka.Services;
using CodeNet.Kafka.Settings;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Kafka.Extensions;

public static class KafkaServiceExtensions
{
    /// <summary>
    /// Add Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaConsumer<TConsumerHandler>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TConsumerHandler : class, IKafkaConsumerHandler<KafkaConsumerService>
        => services.AddKafkaConsumer<KafkaConsumerService, TConsumerHandler>(kafkaSection);

    /// <summary>
    /// Add Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaConsumer<TConsumerService, TConsumerHandler>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TConsumerService : KafkaConsumerService
        where TConsumerHandler : class, IKafkaConsumerHandler<TConsumerService>
    {
        _ = typeof(TConsumerService).Equals(typeof(KafkaConsumerService))
            ? services.Configure<KafkaConsumerOptions>(kafkaSection)
            : services.Configure<KafkaConsumerOptions<TConsumerService, Null, string>>(kafkaSection);

        services.AddSingleton<IKafkaConsumerHandler<TConsumerService>, TConsumerHandler>();
        return services.AddSingleton<TConsumerService>();
    }

    /// <summary>
    /// Add Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaConsumer<TConsumerService, TValue, TConsumerHandler>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TConsumerService : KafkaConsumerService<TValue>
        where TConsumerHandler : class, IKafkaConsumerHandler<TConsumerService, TValue>
    {
        _ = typeof(TConsumerService).Equals(typeof(KafkaConsumerService<TValue>))
            ? services.Configure<KafkaConsumerOptions>(kafkaSection)
            : services.Configure<KafkaConsumerOptions<TConsumerService, Null, TValue>>(kafkaSection);

        services.AddSingleton<IKafkaConsumerHandler<TConsumerService, TValue>, TConsumerHandler>();
        return services.AddSingleton<TConsumerService>();
    }

    /// <summary>
    /// Add Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConsumerHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaConsumer<TConsumerService, TKey, TValue, TConsumerHandler>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TConsumerService : KafkaConsumerService<TKey, TValue>
        where TConsumerHandler : class, IKafkaConsumerHandler<TConsumerService, TKey, TValue>
    {
        _ = typeof(TConsumerService).Equals(typeof(KafkaConsumerService<TKey, TValue>))
            ? services.Configure<KafkaConsumerOptions>(kafkaSection)
            : services.Configure<KafkaConsumerOptions<TConsumerService, TKey, TValue>>(kafkaSection);

        services.AddSingleton<IKafkaConsumerHandler<TConsumerService, TKey, TValue>, TConsumerHandler>();
        return services.AddSingleton<TConsumerService>();
    }

    /// <summary>
    /// Add Kafka Producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfigurationSection kafkaSection) => services.AddKafkaProducer<KafkaProducerService>(kafkaSection);

    /// <summary>
    /// Add Kafka Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaProducer<TProducerService>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TProducerService : KafkaProducerService
    {
        _ = typeof(TProducerService).Equals(typeof(KafkaProducerService))
            ? services.Configure<KafkaProducerOptions>(kafkaSection)
            : services.Configure<KafkaProducerOptions<TProducerService, Null, string>>(kafkaSection);
        return services.AddScoped<TProducerService>();
    }

    /// <summary>
    /// Add Kafka Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaProducer<TProducerService, TValue>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TProducerService : KafkaProducerService<TValue>
    {
        _ = typeof(TProducerService).Equals(typeof(KafkaProducerService<TValue>))
            ? services.Configure<KafkaProducerOptions>(kafkaSection)
            : services.Configure<KafkaProducerOptions<TProducerService, Null, TValue>>(kafkaSection);
        return services.AddScoped<TProducerService>();
    }

    /// <summary>
    /// Add Kafka Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="services"></param>
    /// <param name="kafkaSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaProducer<TProducerService, TKey, TValue>(this IServiceCollection services, IConfigurationSection kafkaSection)
        where TProducerService : KafkaProducerService<TKey, TValue>
    {
        _ = typeof(TProducerService).Equals(typeof(KafkaProducerService<TKey, TValue>))
            ? services.Configure<KafkaProducerOptions>(kafkaSection)
            : services.Configure<KafkaProducerOptions<TProducerService, TKey, TValue>>(kafkaSection);
        return services.AddScoped<TProducerService>();
    }

    /// <summary>
    /// Use Kafka Consumer
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseKafkaConsumer(this WebApplication app) => app.UseKafkaConsumer<KafkaConsumerService>();

    /// <summary>
    /// Use Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseKafkaConsumer<TConsumerService>(this WebApplication app)
        where TConsumerService : KafkaConsumerService => app.UseKafkaConsumer<TConsumerService, string>();

    /// <summary>
    /// Use Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseKafkaConsumer<TConsumerService, TValue>(this WebApplication app)
        where TConsumerService : KafkaConsumerService<TValue>
        => app.UseKafkaConsumer<TConsumerService, Null, TValue>();

    /// <summary>
    /// Use Kafka Consumer
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseKafkaConsumer<TConsumerService, TKey, TValue>(this WebApplication app)
        where TConsumerService : KafkaConsumerService<TKey, TValue>
    {
        var serviceScope = app.Services.CreateScope();
        var consumerService = serviceScope.ServiceProvider.GetService<TConsumerService>() ?? throw new NotImplementedException($"'{nameof(TConsumerService)}' not implemented service.");
        if (DependHandler<TConsumerService, TKey, TValue>(serviceScope, consumerService))
            app.Lifetime.ApplicationStarted.Register(consumerService.StartListening);

        return app;
    }

    /// <summary>
    /// Depend Handler
    /// </summary>
    /// <typeparam name="TConsumerService"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="serviceScope"></param>
    /// <param name="consumerService"></param>
    /// <returns></returns>
    private static bool DependHandler<TConsumerService, TKey, TValue>(IServiceScope serviceScope, TConsumerService consumerService)
        where TConsumerService : KafkaConsumerService<TKey, TValue>
    {
        var messageHandlerService = serviceScope.ServiceProvider.GetService<IKafkaConsumerHandler<TConsumerService, TKey, TValue>>();
        if (messageHandlerService is not null)
        {
            consumerService.ReceivedMessage += messageHandlerService.Handler;
            return true;
        }

        return false;
    }
}
