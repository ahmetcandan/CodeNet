using Autofac;
using NetCore.Abstraction;

namespace NetCore.RabbitMQ.Module;

public class RabbitMQModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RabbitMQService>().As<IQService>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}