using Autofac;
using NetCore.Abstraction;
using NetCore.Elasticsearch;

namespace NetCore.RabbitMQ.Module;

/// <summary>
/// Elasticsearch Module
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class ElasticsearchModule<TModel> : Autofac.Module
    where TModel : class, IElasticsearchModel, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ElasticsearchRepository<TModel>>().As<IElasticsearchRepository<TModel>>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}

/// <summary>
/// Elasticsearch Module
/// </summary>
/// <typeparam name="TElasticsearchRepository"></typeparam>
/// <typeparam name="TModel"></typeparam>
public class ElasticsearchModule<TElasticsearchRepository, TModel> : Autofac.Module
    where TModel : class, IElasticsearchModel, new()
    where TElasticsearchRepository : IElasticsearchRepository<TModel>
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TElasticsearchRepository>().As<IElasticsearchRepository<TModel>>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
