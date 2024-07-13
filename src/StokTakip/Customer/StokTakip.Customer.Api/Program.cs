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


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
    .AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"))
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger()
    .AddRabbitMQConsumer(builder.Configuration.GetSection("RabbitMQ"))
    .AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"))
    .AddMongoDB(builder.Configuration.GetSection("MongoDB"))
    .AddElasticsearch(builder.Configuration.GetSection("Elasticsearch"))
    .AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!))
    .AddSqlServer<CustomerDbContext>(builder.Configuration.GetConnectionString("SqlServer")!)
    .AddParameters(c => c.UseInMemoryDatabase("ParameterDB"), builder.Configuration.GetSection("Redis"))
    .AddHealthChecks()
    .AddCodeNetHealthCheck()
    .AddEntityFrameworkHealthCheck<CustomerDbContext>()
    .AddRedisHealthCheck()
    .AddMongoDbHealthCheck()
    .AddRabbitMqHealthCheck(builder.Services, builder.Configuration.GetSection("RabbitMQ"))
    .AddElasticsearchHealthCheck();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IKeyValueRepository, KeyValueMongoRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();
builder.Services.AddScoped<IRabbitMQConsumerHandler<MongoModel>, MessageConsumerHandler>();


var app = builder.Build();
app.UseCodeNet(builder.Configuration.GetSection("Application"))
    .UseRabbitMQConsumer<MongoModel>()
    .UseDistributedCache()
    .UseDistributedLock()
    .UseLogging()
    .UseHealthChecks("/health")
    .UseExceptionHandling();
app.Run();
