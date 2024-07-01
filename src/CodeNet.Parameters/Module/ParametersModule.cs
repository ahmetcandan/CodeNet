using Autofac;
using CodeNet.Core.Module;
using CodeNet.Parameters.Handler;
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
        builder.RegisterModule<MediatRModule<GetParameterHandler>>();
        builder.RegisterType<ParameterManager>().As<IParameterManager>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}