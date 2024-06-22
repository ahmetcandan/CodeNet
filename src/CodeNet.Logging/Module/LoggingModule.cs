using Autofac;
using CodeNet.Logging.Handler;
using MediatR;

namespace CodeNet.Logging.Module;

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