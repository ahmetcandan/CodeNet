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
using StokTakip.Customer.Service.Handler;
using CodeNet.Core.Enums;
using CodeNet.HttpClient.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
    .AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"))
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger()
    //.AddDefaultErrorMessage(builder.Configuration.GetSection("DefaultErrorMessage"))
    //.AddRabbitMQConsumer(builder.Configuration.GetSection("RabbitMQ"))
    //.AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"))
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
//builder.Services.AddScoped<IRabbitMQConsumerHandler<MongoModel>, MessageConsumerHandler>();


var app = builder.Build();
app.UseCodeNet()
    //.UseRabbitMQConsumer<MongoModel>()
    .UseLogging()
    .UseDistributedCache()
    .UseDistributedLock()
    .UseExceptionHandling()
    ;
//app.UseCodeNetHealthChecks("/health");
app.Run();
