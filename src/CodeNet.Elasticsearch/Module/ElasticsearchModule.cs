using Autofac;
using CodeNet.Elasticsearch;

namespace CodeNet.RabbitMQ.Module;

/// <summary>
/// </summary>
/// <typeparam name="TElasticsearchDBContext"></typeparam>
public class ElasticsearchModule<TElasticsearchDBContext> : Autofac.Module
    where TElasticsearchDBContext : ElasticsearchDbContext
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
        builder.RegisterType<ElasticsearchDbContext>().As<ElasticsearchDbContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
