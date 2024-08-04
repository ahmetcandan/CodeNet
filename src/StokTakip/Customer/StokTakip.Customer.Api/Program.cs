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
using CodeNet.StackExchange.Redis.Extensions;
using CodeNet.StackExchange.Redis.Services;
using CodeNet.Parameters.MongoDB.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
    .AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"))
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger()
    .AddDefaultErrorMessage(builder.Configuration.GetSection("DefaultErrorMessage"))
    //.AddRabbitMQConsumer<ConsumerServiceA, MessageHandlerA>(builder.Configuration.GetSection("RabbitMQA"))
    //.AddRabbitMQProducer<ProducerServiceA>(builder.Configuration.GetSection("RabbitMQA"))
    //.AddRabbitMQConsumer<ConsumerServiceB, MessageHandlerB>(builder.Configuration.GetSection("RabbitMQB"))
    //.AddRabbitMQProducer<ProducerServiceB>(builder.Configuration.GetSection("RabbitMQB"))
    //.AddStackExcahangeConsumer<RedisConsumerServiceA, RedisMessageHandlerA>(builder.Configuration.GetSection("StackExchangeA"))
    //.AddStackExcahangeConsumer<RedisConsumerServiceB, RedisMessageHandlerB>(builder.Configuration.GetSection("StackExchangeB"))
    //.AddStackExcahangeProducer<RedisProducerServiceA>(builder.Configuration.GetSection("StackExchangeA"))
    //.AddStackExcahangeProducer<RedisProducerServiceB>(builder.Configuration.GetSection("StackExchangeB"))
    .AddMongoDB<AMongoDbContext>(builder.Configuration.GetSection("AMongoDB"))
    .AddMongoDB<BMongoDbContext>(builder.Configuration.GetSection("BMongoDB"))
    .AddMongoDB(builder.Configuration.GetSection("BMongoDB"))
    //.AddElasticsearch(builder.Configuration.GetSection("Elasticsearch"))
    .AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!))
    .AddHttpRequest()
    .AddParameters(options =>
    {
        options.AddMongoDb(builder.Configuration.GetSection("BMongoDB"));
        options.AddRedis(builder.Configuration.GetSection("Redis"));
    })
    .AddHealthChecks(options =>
    {
        options.AddCodeNetHealthCheck();
        options.AddEntityFrameworkHealthCheck<CustomerDbContext>();
        options.AddRedisHealthCheck();
        options.AddMongoDbHealthCheck();
        options.AddRabbitMqHealthCheck(builder.Services, builder.Configuration.GetSection("RabbitMQ"));
        options.AddElasticsearchHealthCheck();
    })
    .AddBackgroundJob(options =>
    {
        options.AddRedis(builder.Configuration.GetSection("Redis"));
        options.AddJob<TestService1>(new CodeNet.BackgroundJob.Settings.JobOptions { CronExpression = "*/3 */5 * * *", ExpryTime = TimeSpan.FromSeconds(1) });
        options.AddDbContext(c => c.UseSqlServer(builder.Configuration.GetConnectionString("BackgroundService")!));
    });

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IKeyValueRepository, KeyValueMongoRepository>();
builder.Services.AddScoped<IAKeyValueRepository, AKeyValueMongoRepository>();
builder.Services.AddScoped<IBKeyValueRepository, BKeyValueMongoRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();

var app = builder.Build();
app.UseCodeNet();
app.UseLogging();
app.UseDistributedCache();
app.UseDistributedLock();
app.UseExceptionHandling();
app.UseCodeNetHealthChecks("/health");
app.UseBackgroundService("/job");
//app.UseRabbitMQConsumer<ConsumerServiceA>();
//app.UseRabbitMQConsumer<ConsumerServiceB>();
//app.UseStackExcahangeConsumer<RedisConsumerServiceA>();
//app.UseStackExcahangeConsumer<RedisConsumerServiceB>();
app.Run();
