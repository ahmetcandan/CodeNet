using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Repository;
using StokTakip.Product.Service;

var builder = WebApplication.CreateBuilder(args);
builder.AddCodeNet("Application");
builder.AddAuthenticationWithAsymmetricKey("JWT");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddDbContext<ProductDbContext>("SqlServer");
builder.AddLogging();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
app.UseCodeNet(builder.Configuration, "Application");
app.Run();
