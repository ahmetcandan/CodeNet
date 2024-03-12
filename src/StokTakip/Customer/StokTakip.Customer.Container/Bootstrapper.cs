using Autofac;
using Microsoft.EntityFrameworkCore;
using NetCore.Container;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Container.Modules;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Service;

namespace StokTakip.Customer.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule>();
        builder.RegisterModule<RepositoryModule>();

        builder.RegisterType<CustomerDbContext>().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
