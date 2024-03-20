using Autofac;
using Microsoft.EntityFrameworkCore;
using NetCore.Container;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Repository;
using StokTakip.Product.Service;
using StokTakip.Product.Service.Handler;

namespace StokTakip.Product.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule<GetProductHandler>>();

        builder.RegisterType<ProductDbContext>().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
        builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
