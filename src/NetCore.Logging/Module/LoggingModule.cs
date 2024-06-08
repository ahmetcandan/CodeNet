using Autofac;
using MediatR;
using NetCore.Abstraction;

namespace NetCore.Logging.Module;

/// <summary>
/// Logging Module
/// </summary>
public class LoggingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(LoggingHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterType<AppLogger>().As<IAppLogger>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}