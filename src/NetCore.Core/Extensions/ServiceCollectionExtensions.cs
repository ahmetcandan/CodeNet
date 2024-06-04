using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.Abstraction.Model;
using Swashbuckle.AspNetCore.Filters;

namespace NetCore.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder UseNetCoreContainer(this IHostBuilder hostBuilder, Action<ContainerBuilder> configureDelegate)
    {
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        return hostBuilder.ConfigureContainer(configureDelegate);
    }

    public static WebApplicationBuilder AddNetCore(this WebApplicationBuilder webBuilder, string sectionName)
    {
        var applicationSettings = webBuilder.Configuration.GetSection(sectionName).Get<ApplicationSettings>()!;
        webBuilder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(applicationSettings.Version, new OpenApiInfo { Title = applicationSettings.Title, Version = applicationSettings.Version });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
              Enter 'Bearer' [space] and then your token in the text input below.
              Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        webBuilder.Services.AddControllers();
        webBuilder.Services.AddEndpointsApiExplorer();

        return webBuilder;
    }

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder webBuilder, string sectionName, string publicKeyPath)
    {
        var authenticationSettings = webBuilder.Configuration.GetSection(sectionName).Get<AuthenticationSettings>()!;
        var rsa = AsymmetricKeyEncryption.CreateRSA(publicKeyPath);
        webBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidIssuer = authenticationSettings.ValidIssuer,
                ValidateIssuer = true,
                ValidAudience = authenticationSettings.ValidAudience,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        webBuilder.Services.AddHttpContextAccessor();
        return webBuilder;
    }

    public static WebApplicationBuilder AddSqlServer(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddSqlServer<DbContext>(connectionName);
        return webBuilder;
    }

    public static WebApplicationBuilder AddSqlServer<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) where TDbContext : DbContext
    {
        webBuilder.Services.AddDbContext<TDbContext>(options => options.UseSqlServer(webBuilder.Configuration.GetConnectionString(connectionName)));
        return webBuilder;
    }

    public static WebApplication UseNetCore(this WebApplication app, IConfiguration configuration, string sectionName)
    {
        var applicationSettings = configuration.GetSection(sectionName).Get<ApplicationSettings>()!;

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseExceptionHandler("/Error");
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{applicationSettings.Version}/swagger.json", $"{applicationSettings.Title} {applicationSettings.Version}"));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
