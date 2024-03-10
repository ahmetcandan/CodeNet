using Autofac;
using AutoMapper;
using MediatR;
using NetCore.Container;
using StokTakip.Product.Service.Mapper;
using System.Reflection;

namespace StokTakip.Product.Container.Modules
{
    public class MediatRModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(Service.Handler.CreateProductHandler).Assembly)
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
}
