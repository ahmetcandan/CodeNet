using Autofac;
using AutoMapper;
using MediatR;
using NetCore.Container;
using StokTakip.Customer.Service.Mapper;
using System.Reflection;

namespace StokTakip.Customer.Container.Modules;

public class MediatRModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(Service.Handler.CreateCustomerHandler).Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(LoggingHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ExceptionHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterType(typeof(Mapper)).As(typeof(IMapper)).AsSelf().InstancePerLifetimeScope();
        builder.RegisterType(typeof(AutoMapperConfiguration)).As(typeof(IAutoMapperConfiguration)).AsSelf().InstancePerLifetimeScope();

        base.Load(builder);
    }
}
