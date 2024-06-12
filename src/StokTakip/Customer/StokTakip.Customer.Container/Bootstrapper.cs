using Autofac;
using NetCore.Container.Module;
using NetCore.ExceptionHandling.Module;
using NetCore.Logging.Module;
using NetCore.Mapper.Module;
using NetCore.Abstraction.Module;
using NetCore.Redis.Module;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Service;
using StokTakip.Customer.Service.Handler;
using StokTakip.Customer.Service.Mapper;
using NetCore.RabbitMQ.Module;
using StokTakip.Customer.Contract.Model;
using NetCore.Abstraction;

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
