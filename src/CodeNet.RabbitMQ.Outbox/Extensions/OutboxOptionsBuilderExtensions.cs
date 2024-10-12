using CodeNet.Outbox.Builder;
using CodeNet.RabbitMQ.Extensions;
using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Configuration;

namespace CodeNet.RabbitMQ.Outbox.Extensions;

public static class OutboxOptionsBuilderExtensions
{
    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static OutboxOptionsBuilder AddRabbitMQProducer(this OutboxOptionsBuilder builder, IConfigurationSection configuration)
    {
        builder.Services.AddRabbitMQProducer(configuration);
        return builder;
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static OutboxOptionsBuilder AddRabbitMQProducer<TProducerService>(this OutboxOptionsBuilder builder, IConfigurationSection configuration)
        where TProducerService : RabbitMQProducerService
    {
        builder.Services.AddRabbitMQProducer<TProducerService>(configuration);
        return builder;
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static OutboxOptionsBuilder AddRabbitMQProducer(this OutboxOptionsBuilder builder, RabbitMQProducerOptions options)
    {
        builder.Services.AddRabbitMQProducer(options);
        return builder;
    }

    /// <summary>
    /// Add RabbitMQ Producer
    /// </summary>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static OutboxOptionsBuilder AddRabbitMQProducer<TProducerService>(this OutboxOptionsBuilder builder, RabbitMQProducerOptions<TProducerService> options)
        where TProducerService : RabbitMQProducerService
    {
        builder.Services.AddRabbitMQProducer(options);
        return builder;
    }
}
