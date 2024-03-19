﻿using Autofac;
using NetCore.Abstraction;
using NetCore.Core;
using NetCore.Elasticsearch;
using NetCore.Logging;
using NetCore.RabbitMQ;
using NetCore.Redis;

namespace NetCore.Container;

public class NetCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {   
        builder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
        builder.RegisterType<ElasticsearchRepository>().As<IElasticsearchRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RedisCacheRepository>().As<ICacheRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RabbitMQService>().As<IQService>().InstancePerLifetimeScope();
        builder.RegisterType<AppLogger>().As<IAppLogger>().InstancePerLifetimeScope();

        base.Load(builder);
    }
}
