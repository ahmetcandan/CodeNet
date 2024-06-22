﻿using Autofac;
using MediatR;
using CodeNet.Redis.Handler;
using CodeNet.Redis.Repositories;

namespace CodeNet.Redis.Module;

/// <summary>
/// Redis Distributed Cache Module
/// </summary>
public class RedisDistributedCacheModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(CacheHandler<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterType<RedisCacheRepository>().As<ICacheRepository>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
