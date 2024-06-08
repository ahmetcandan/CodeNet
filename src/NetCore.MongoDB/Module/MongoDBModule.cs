using Autofac;
using NetCore.Abstraction.Model;

namespace NetCore.MongoDB.Module;

public class MongoDBModule<TMongoDB, TModel> : Autofac.Module
        where TMongoDB : BaseMongoRepository<TModel>
        where TModel : INoSqlModel, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TMongoDB>().As<TMongoDB>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}