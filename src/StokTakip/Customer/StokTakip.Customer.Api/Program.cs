using StokTakip.Customer.Repository;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.Elasticsearch.Extensions;
using CodeNet.MongoDB.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.ExceptionHandling.Extensions;
using CodeNet.HealthCheck.Extensions;
using CodeNet.HealthCheck.Redis.Extensions;
using CodeNet.HealthCheck.EntityFramework.Extensions;
using CodeNet.HealthCheck.MongoDB.Extensions;
using CodeNet.HealthCheck.RabbitMQ.Extensions;
using CodeNet.HealthCheck.Elasticsearch.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Parameters.Extensions;
using CodeNet.EntityFramework.InMemory.Extensions;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Service;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Service.Mapper;
using CodeNet.RabbitMQ.Extensions;
using StokTakip.Customer.Contract.Model;
using CodeNet.RabbitMQ.Services;
using CodeNet.Core.Enums;
using CodeNet.HttpClient.Extensions;
using StokTakip.Customer.Model;
using StokTakip.Customer.Service.QueueService;
using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Extensions;
using Quartz;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
    .AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"))
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger()
    //.AddDefaultErrorMessage(builder.Configuration.GetSection("DefaultErrorMessage"))
    //.AddRabbitMQConsumer<ConsumerServiceA>(builder.Configuration.GetSection("RabbitMQA"))
    //.AddRabbitMQProducer<ProducerServiceA>(builder.Configuration.GetSection("RabbitMQA"))
    //.AddRabbitMQConsumer<ConsumerServiceB>(builder.Configuration.GetSection("RabbitMQB"))
    //.AddRabbitMQProducer<ProducerServiceB>(builder.Configuration.GetSection("RabbitMQB"))
    .AddMongoDB<AMongoDbContext>(builder.Configuration.GetSection("AMongoDB"))
    .AddMongoDB<BMongoDbContext>(builder.Configuration.GetSection("BMongoDB"))
    .AddMongoDB(builder.Configuration.GetSection("BMongoDB"))
    //.AddElasticsearch(builder.Configuration.GetSection("Elasticsearch"))
    .AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!))
    //.AddSqlServer<CustomerDbContext>(builder.Configuration.GetConnectionString("SqlServer")!)
    //.AddParameters(c => c.UseInMemoryDatabase("ParameterDB"), builder.Configuration.GetSection("Redis"))
    .AddHttpRequest()
    .AddCodeNetHealthCheck()
        .AddEntityFrameworkHealthCheck<CustomerDbContext>()
        .AddRedisHealthCheck()
        .AddMongoDbHealthCheck()
        .AddRabbitMqHealthCheck(builder.Services, builder.Configuration.GetSection("RabbitMQ"))
        .AddElasticsearchHealthCheck()
    ;

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IKeyValueRepository, KeyValueMongoRepository>();
builder.Services.AddScoped<IAKeyValueRepository, AKeyValueMongoRepository>();
builder.Services.AddScoped<IBKeyValueRepository, BKeyValueMongoRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();
builder.Services.AddBackgroundService<TestService1>("*/17 * * * * ? *")
    .AddBackgroundService<TestService2>("*/13 * * * * ? *")
    .AddBackgroundJobRedis(builder.Configuration.GetSection("Redis"));
//builder.Services.AddScoped<IRabbitMQConsumerHandler<ConsumerServiceA>, MessageHandlerA>();
//builder.Services.AddScoped<IRabbitMQConsumerHandler<ConsumerServiceB>, MessageHandlerB>();

var app = builder.Build();
app.UseCodeNet();
app.UseLogging();
app.UseDistributedCache();
app.UseDistributedLock();
app.UseExceptionHandling();
app.UseCodeNetHealthChecks("/health");
//app.UseBackgroundService<TestService>();
app.UseBackgroundService();
//app.UseRabbitMQConsumer<ConsumerServiceA>();
//app.UseRabbitMQConsumer<ConsumerServiceB>();
app.Run();
