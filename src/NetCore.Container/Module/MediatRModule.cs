using Autofac;
using MediatR;
using System.Reflection;

namespace NetCore.Container.Module;

/// <summary>
/// MediatR Module
/// </summary>
/// <typeparam name="THandlerType">Type of Handler</typeparam>
public class MediatRModule<THandlerType> : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(THandlerType).Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        base.Load(builder);
    }
}

