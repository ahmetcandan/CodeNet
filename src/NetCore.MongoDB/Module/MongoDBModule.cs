using Autofac;

namespace NetCore.MongoDB.Module;

public class MongoDBModule<TMongoDBRepository, TModel> : Autofac.Module
        where TMongoDBRepository : BaseMongoRepository<TModel>
        where TModel : class, new()
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TMongoDBRepository>().As<TMongoDBRepository>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}