using Autofac;
using CodeNet.Parameters.Manager;
using CodeNet.Redis.Module;

namespace CodeNet.Parameters.Module;

/// <summary>
/// Parameters Module
/// </summary>
public class ParametersModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<RedisDistributedCacheModule>();
        builder.RegisterType<ParameterManager>().As<IParameterManager>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}