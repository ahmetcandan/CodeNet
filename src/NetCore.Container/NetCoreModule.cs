using Autofac;
using AutoMapper;
using MediatR;
using NetCore.Abstraction;
using NetCore.Core;
using NetCore.Logging;
using NetCore.RabbitMQ;
using NetCore.Redis;

namespace NetCore.Container;

public class NetCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {

        builder.RegisterGeneric(typeof(LoggingHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ExceptionHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(CacheHandler<,>)).As(typeof(IPipelineBehavior<,>));
        
        builder.RegisterType(typeof(Mapper)).As(typeof(IMapper)).AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
        builder.RegisterType<RedisCacheRepository>().As<ICacheRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RabbitMQService>().As<IQService>().InstancePerLifetimeScope();
        builder.RegisterType<AppLogger>().As<IAppLogger>().InstancePerLifetimeScope();

        base.Load(builder);
    }
}
