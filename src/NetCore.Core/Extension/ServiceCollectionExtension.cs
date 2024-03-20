using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace NetCore.Core.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection service, string title, string version = "v1")
    {
        service.AddSwaggerGen(c =>
         {
             c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
              Enter 'Bearer' [space] and then your token in the text input below.
              \r\n\r\nExample: 'Bearer 12345abcdef'",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer"
             });

             c.OperationFilter<SecurityRequirementsOperationFilter>();
         });
        return service;
    }

    public static IServiceCollection AddRedisSettings(this IServiceCollection service, string configuration, string instanceName = "master")
    {
        service.AddStackExchangeRedisCache(option =>
         {
             option.Configuration = configuration;
             option.InstanceName = instanceName;
         });
        return service;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection service, string validAudience, string validIssuer, string secret)
    {
        service.AddAuthentication(options =>
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
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = validAudience,
                ValidIssuer = validIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };
        });
        return service;
    }
}
