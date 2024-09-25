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
using CodeNet.BackgroundJob.Settings;
using CodeNet.MakerChecker.Extensions;
using CodeNet.Mapper.Extensions;
using StokTakip.Customer.Contract.Request;
using CodeNet.Mapper.Configurations;
using StokTakip.Customer.Contract.Response;
using CodeNet.EntityFramework.MySQL.Extensions;
using CodeNet.HealthCheck.Kafka.Extensions;
using CodeNet.SignalR.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"), options =>
    {
        options.AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"));
    })
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger()
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
    .AddDbContext<CustomerDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MySQL")!))
    .AddMySQL<CustomerDbContext>("")
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
        options.AddKafkaHealthCheck(builder.Services, builder.Configuration.GetSection("Kafka")!);
        //options.AddRabbitMqHealthCheck(builder.Services, builder.Configuration.GetSection("RabbitMQ"));
        options.AddElasticsearchHealthCheck();
    })
    .AddBackgroundJob(options =>
    {
        //options.AddRedis(builder.Configuration.GetSection("Redis"));
        //options.AddScheduleJob<TestService1>("TestService1", TimeSpan.FromSeconds(115), new() { ExpryTime = TimeSpan.FromSeconds(1) });
        options.AddScheduleJob<TestService2>("TestService2", "0 */5 * * *", new() { ExpryTime = TimeSpan.FromSeconds(1) });
        options.AddScheduleJob<TestService3>("TestService3", new() { ExpryTime = TimeSpan.FromSeconds(1) });
        //options.AddDbContext(c => c.UseSqlServer(builder.Configuration.GetConnectionString("BackgroundService")!));
        options.AddCurrentAuth();
        options.AddBasicAuth(new Dictionary<string, string> { { "admin", "Admin123!" } });
    })
    .AddMapper(c => 
    {
        c.SetMaxDepth(3);

        c.CreateMap<CreateCustomerRequest, Customer>()
            .Map(s => s.Name, d => d.Name)
            .Map(s => s.Number, d => d.No);

        c.CreateMap<CustomerResponse, Customer>()
            .Map(s => s.Name, d => d.Name)
            .Map(s => s.Number, d => d.No)
            .MaxDepth(4);
    })
    ;
//builder.Services.AddSignalRNotification();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IKeyValueRepository, KeyValueMongoRepository>();
builder.Services.AddScoped<IAKeyValueRepository, AKeyValueMongoRepository>();
builder.Services.AddScoped<IBKeyValueRepository, BKeyValueMongoRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();


var app = builder.Build();
app.UseCodeNet(options =>
{
    options.UseAuthentication();
    options.UseAuthorization();
});
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseLogging();
app.UseDistributedCache();
app.UseDistributedLock();
app.UseExceptionHandling();
app.UseCodeNetHealthChecks();
app.UseBackgroundService();
//app.UseSignalR<THub>("/tst");
//app.UseRabbitMQConsumer<ConsumerServiceA>();
//app.UseRabbitMQConsumer<ConsumerServiceB>();
//app.UseStackExcahangeConsumer<RedisConsumerServiceA>();
//app.UseStackExcahangeConsumer<RedisConsumerServiceB>();
app.Run();
