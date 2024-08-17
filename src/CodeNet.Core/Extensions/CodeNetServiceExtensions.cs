﻿using CodeNet.Core.Enums;
using CodeNet.Core.Security;
using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace CodeNet.Core.Extensions;

public static class CodeNetServiceExtensions
{
    private const string _devCorsPolicyName = "AllowDevOrigin";
    private const string _codeNet = @"
   ___            _         _  _         _   
  / __|  ___   __| |  ___  | \| |  ___  | |_ 
 | (__  / _ \ / _` | / -_) | .` | / -_) |  _|
  \___| \___/ \__,_| \___| |_|\_| \___|  \__|
                                             ";

    /// <summary>
    /// Use CodeNet
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplication UseCodeNet(this WebApplication app)
    {
        var applicationSettings = app.Services.GetService<IOptions<ApplicationSettings>>();
        if (applicationSettings?.Value is null)
            throw new NullReferenceException("ApplicationSettings is not implemented. Use the builder.Services.AddCodeNet(...) method.");

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/{applicationSettings.Value.Version}/swagger.json", $"{applicationSettings.Value.Title} {applicationSettings.Value.Version}"));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors(_devCorsPolicyName);
        return app;
    }

    /// <summary>
    /// Add CodeNet Configuration
    /// This method contains AddCodeNetContext.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddCodeNet(this IServiceCollection services, IConfiguration configuration, string sectionName, Action<CodeNetOptionsBuilder> action = null)
    {
        return services.AddCodeNet(configuration.GetSection(sectionName), action);
    }

    /// <summary>
    /// Add CodeNet Configuration
    /// This method contains AddCodeNetContext.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddCodeNet(this IServiceCollection services, IConfigurationSection configurationSection, Action<CodeNetOptionsBuilder> action = null)
    {
        Console.WriteLine(_codeNet);

        services.Configure<ApplicationSettings>(configurationSection);
        var applicationSettings = configurationSection.Get<ApplicationSettings>() ?? throw new ArgumentNullException($"'{configurationSection.Path}' is null or empty in appSettings.json");
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(applicationSettings.Version, new OpenApiInfo { Title = applicationSettings.Title, Version = applicationSettings.Version });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                  Enter 'Bearer' [space] and then your token in the text input below.
                  Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();

        if (action is not null)
        {
            var builder = new CodeNetOptionsBuilder(services);
            builder.AddCodeNetContext();
            action(builder);
        }
        else
            new CodeNetOptionsBuilder(services).AddCodeNetContext();

        return services.AddCors(options =>
        {
            options.AddPolicy(_devCorsPolicyName,
                builder =>
                {
                    builder.WithOrigins("https://localhost:7236")
                           .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
    }
}
