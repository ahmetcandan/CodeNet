using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCore.Container;
using NetCore.Core.Extension;
using NetCore.EntityFramework;
using NetCore.EntityFramework.Model;
using NetCore.Identity.Handler;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<NetCoreModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();

    containerBuilder.RegisterType<IdentityTokenManager>().As<IIdentityTokenManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityUserManager>().As<IIdentityUserManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityRoleManager>().As<IIdentityRoleManager>().InstancePerLifetimeScope();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("NetCore | Identity API");
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddRedisSettings(builder.Configuration["Redis:Url"]!);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(builder.Configuration["JWT:ValidAudience"]!, builder.Configuration["JWT:ValidIssuer"]!, "public_key.pem");
builder.Services.AddHttpContextAccessor();

var uri = new Uri("https://192.168.1.47"); 
var ipAddresses = Dns.GetHostAddresses(uri.Host);

var endPoints = new List<RedLockEndPoint>
{
    new() { EndPoint = new IPEndPoint(ipAddresses[0], 6379) }
};
builder.Services.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseExceptionHandler("/Error");
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API v1"));
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
