using Autofac;
using Microsoft.EntityFrameworkCore;
using NetCore.Container;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Service;
using StokTakip.Customer.Service.Handler;
using StokTakip.Customer.Service.Mapper;

namespace StokTakip.Customer.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule<GetCustomerHandler>>();

        builder.RegisterType(typeof(AutoMapperConfiguration)).As(typeof(IAutoMapperConfiguration)).AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<CustomerDbContext>().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
