using Autofac;
using MediatR;

namespace CodeNet.Redis.Module;

/// <summary>
/// Redis Distributed Lock Module
/// </summary>
public class RedisDistributedLockModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(LockHandler<,>)).As(typeof(IPipelineBehavior<,>));
        base.Load(builder);
    }
}
