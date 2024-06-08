using Autofac;
using NetCore.Container.Module;
using NetCore.Logging.Module;
using NetCore.Mapper.Module;
using NetCore.Redis.Module;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Repository;
using StokTakip.Campaign.Service;
using StokTakip.Campaign.Service.Handler;

namespace StokTakip.Campaign.Container;

public class Bootstrapper
{
    public static ILifetimeScope? Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule<GetCampaignHandler>>();
        builder.RegisterModule<MapperModule>();
        builder.RegisterModule<RedisDistributedCacheModule>();
        builder.RegisterModule<RedisDistributedLockModule>();
        builder.RegisterModule<LoggingModule>();
        builder.RegisterType<CampaignRepository>().As<ICampaignRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
