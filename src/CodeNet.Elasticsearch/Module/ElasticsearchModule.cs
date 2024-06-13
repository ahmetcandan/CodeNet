using Autofac;
using CodeNet.Elasticsearch;

namespace CodeNet.RabbitMQ.Module;

/// <summary>
/// </summary>
/// <typeparam name="TElasticsearchDBContext"></typeparam>
public class ElasticsearchModule<TElasticsearchDBContext> : Autofac.Module
    where TElasticsearchDBContext : ElasticsearchDBContext
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TElasticsearchDBContext>().As<TElasticsearchDBContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}

/// <summary>
/// Elasticsearch Module
/// </summary>
public class ElasticsearchModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ElasticsearchDBContext>().As<ElasticsearchDBContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
