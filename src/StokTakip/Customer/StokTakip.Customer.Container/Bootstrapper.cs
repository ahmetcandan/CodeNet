using Autofac;
using CodeNet.Container.Module;
using CodeNet.ExceptionHandling.Module;
using CodeNet.Logging.Module;
using CodeNet.Mapper.Module;
using CodeNet.Redis.Module;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Service;
using StokTakip.Customer.Service.Handler;
using StokTakip.Customer.Service.Mapper;
using CodeNet.RabbitMQ.Module;
using StokTakip.Customer.Contract.Model;
using CodeNet.Abstraction;
using CodeNet.MongoDB.Module;

namespace StokTakip.Customer.Container;

public class Bootstrapper
{
    public static ILifetimeScope? Container { get; private set; }

    public static void RegisterModules(ContainerBuilder builder)
    {
        builder.RegisterModule<NetCoreModule>();
        builder.RegisterModule<MediatRModule<GetCustomerHandler>>();
        builder.RegisterModule<MapperModule>();
        builder.RegisterModule<RedisDistributedCacheModule>();
        builder.RegisterModule<RedisDistributedLockModule>();
        builder.RegisterModule<LoggingModule>();
        builder.RegisterModule<MongoDBModule>();
        builder.RegisterModule<RabbitMQProducerModule<KeyValueModel>>();
        builder.RegisterModule<RabbitMQConsumerModule<KeyValueModel>>();
        builder.RegisterModule<ExceptionHandlingModule>();
        builder.RegisterModule<ElasticsearchModule>();
        builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<KeyValueMongoRepository>().As<IKeyValueRepository>().InstancePerLifetimeScope();
        builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
        builder.RegisterType<AutoMapperConfiguration>().As<IAutoMapperConfiguration>().InstancePerLifetimeScope();
        builder.RegisterType<MessageHandler>().As<IRabbitMQConsumerHandler<KeyValueModel>>().InstancePerLifetimeScope();
    }

    public static void SetContainer(ILifetimeScope lifetimeScope)
    {
        Container = lifetimeScope;
    }
}
