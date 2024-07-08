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


var builder = WebApplication.CreateBuilder(args);
builder.AddCodeNet("Application");
builder.AddAuthenticationWithAsymmetricKey("JWT");
builder.AddRedisDistributedCache("Redis");
//builder.AddRedisDistributedLock("Redis");
builder.AddLogging();
//builder.AddRabbitMQConsumer("RabbitMQ");
//builder.AddRabbitMQProducer("RabbitMQ");
//builder.AddMongoDB("MongoDB");
//builder.AddElasticsearch("Elasticsearch");
builder.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration, "SqlServer"));
builder.AddInMemoryDB("asd");
builder.AddParameters(c => c.UseInMemoryDatabase("ParameterDB"), "Redis");
builder.Services.AddHealthChecks()
    .AddCodeNetHealthCheck()
    .AddEntityFrameworkHealthCheck<CustomerDbContext>()
    .AddRedisHealthCheck()
    .AddMongoDbHealthCheck()
    .AddRabbitMqHealthCheck(builder, "RabbitMQ")
    .AddElasticsearchHealthCheck();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
//builder.Services.AddScoped<IKeyValueRepository, KeyValueMongoRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();
//builder.Services.AddScoped<IRabbitMQConsumerHandler<MongoModel>, MessageConsumerHandler>();


var app = builder.Build();
app.UseCodeNet(builder.Configuration, "Application");
//app.UseRabbitMQConsumer<MongoModel>();
app.UseDistributedCache();
app.UseDistributedLock();
app.UseLogging();
app.UseExceptionHandling();
app.UseHealthChecks("/health");
app.Run();
