using Autofac;
using NetCore.Abstraction;

namespace NetCore.RabbitMQ.Module;

public class RabbitMQModule<TRabbitMQService, TData> : Autofac.Module
    where TRabbitMQService : IRabbitMQService<TData>
    where TData : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TRabbitMQService>().As<TRabbitMQService>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}