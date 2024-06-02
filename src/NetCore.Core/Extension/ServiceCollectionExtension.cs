﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.Abstraction.Model;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace NetCore.Core.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddNetCore(this IServiceCollection services, ApplicationSettings applicationSettings) 
    {
        services.AddSwaggerGen(c =>
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
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection AddRedisSettings(this IServiceCollection service, string hostname, int port, string instanceName = "master")
    {
        //Cache
        service.AddStackExchangeRedisCache(option =>
         {
             option.Configuration = $"{hostname}:{port}";
             option.InstanceName = instanceName;
         });

        //Lock
        var ipAddresses = Dns.GetHostAddresses(hostname);
        var endPoints = new List<RedLockEndPoint>
        {
            new() { EndPoint = new IPEndPoint(ipAddresses[0], port) }
        };
        service.AddSingleton<IDistributedLockFactory>(_ => RedLockFactory.Create(endPoints));

        return service;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection service, string validAudience, string validIssuer, string publicKeyPath)
    {
        var rsa = AsymmetricKeyEncryption.CreateRSA(publicKeyPath);
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
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidIssuer = validIssuer,
                ValidateIssuer = true,
                ValidAudience = validAudience,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        service.AddHttpContextAccessor();
        return service;
    }

    public static IServiceCollection AddSqlServer(this IServiceCollection service, string connectionString)
    {
        return service.AddSqlServer<DbContext>(connectionString);
    }

    public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection service, string connectionString) where TDbContext : DbContext
    {
        return service.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
    }

    public static WebApplication UseNetCore(this WebApplication app, ApplicationSettings applicationSettings)
    {
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
