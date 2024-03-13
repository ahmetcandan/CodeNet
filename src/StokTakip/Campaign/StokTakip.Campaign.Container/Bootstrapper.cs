using Autofac;
using Microsoft.EntityFrameworkCore;
using NetCore.Container;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Container.Modules;
using StokTakip.Campaign.Repository;
using StokTakip.Campaign.Service;

namespace StokTakip.Campaign.Container;

public class Bootstrapper
{
    public static ILifetimeScope Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule>();
        builder.RegisterModule<RepositoryModule>();

        builder.RegisterType<CampaignDbContext>().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<CampaignRepository>().As<ICampaignRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
