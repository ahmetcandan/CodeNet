using Autofac;
using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Module;

/// <summary>
/// RabbitMQ Producer Module
/// </summary>
/// <typeparam name="TRabbitMQProducerService"></typeparam>
/// <typeparam name="TData"></typeparam>
public class RabbitMQProducerModule<TRabbitMQProducerService, TData> : Autofac.Module
    where TRabbitMQProducerService : IRabbitMQProducerService<TData>
    where TData : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TRabbitMQProducerService>().As<TRabbitMQProducerService>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}

/// <summary>
/// RabbitMQ Producer Module
/// </summary>
/// <typeparam name="TData"></typeparam>
public class RabbitMQProducerModule<TData> : Autofac.Module
    where TData : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RabbitMQProducerService<TData>>().As<IRabbitMQProducerService<TData>>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}

/// <summary>
/// RabbitMQ Producer Module
/// </summary>
/// <typeparam name="TRabbitMQConsumerService"></typeparam>
/// <typeparam name="TData"></typeparam>
public class RabbitMQConsumerModule<TRabbitMQConsumerService, TData> : Autofac.Module
    where TRabbitMQConsumerService : IRabbitMQConsumerService<TData>
    where TData : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TRabbitMQConsumerService>().As<TRabbitMQConsumerService>().SingleInstance();
        base.Load(builder);
    }
}

/// <summary>
/// RabbitMQ Producer Module
/// </summary>
/// <typeparam name="TData"></typeparam>
public class RabbitMQConsumerModule<TData> : Autofac.Module
    where TData : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RabbitMQConsumerService<TData>>().As<IRabbitMQConsumerService<TData>>().SingleInstance();
        base.Load(builder);
    }
}