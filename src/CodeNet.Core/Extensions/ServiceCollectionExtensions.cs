using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodeNet.Core.Security;
using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CodeNet.Core.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Use CodeNet Container
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <param name="configureDelegate"></param>
    /// <returns></returns>
    public static IHostBuilder UseCodeNetContainer(this IHostBuilder hostBuilder, Action<ContainerBuilder> configureDelegate)
    {
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        return hostBuilder.ConfigureContainer(configureDelegate);
    }

    /// <summary>
    /// Add CodeNet Configuration
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type ApplicationSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddCodeNet(this WebApplicationBuilder webBuilder, string sectionName)
    {
        Console.WriteLine(@"
   ___            _         _  _         _   
  / __|  ___   __| |  ___  | \| |  ___  | |_ 
 | (__  / _ \ / _` | / -_) | .` | / -_) |  _|
  \___| \___/ \__,_| \___| |_|\_| \___|  \__|
                                             ");
        var applicationSettings = webBuilder.Configuration.GetSection(sectionName).Get<ApplicationSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        
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
        webBuilder.Services.AddHttpContextAccessor();

        return webBuilder;
    }

    /// <summary>
    /// Add Authentication
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type AuthenticationSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder webBuilder, string sectionName)
    {
        var authenticationSettings = webBuilder.Configuration.GetSection(sectionName).Get<AuthenticationSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");
        var rsa = AsymmetricKeyEncryption.CreateRSA(authenticationSettings.PublicKeyPath);
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

        return webBuilder;
    }

    /// <summary>
    /// Use CodeNet
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type ApplicationSettings</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplication UseCodeNet(this WebApplication app, IConfiguration configuration, string sectionName)
    {
        var applicationSettings = configuration.GetSection(sectionName).Get<ApplicationSettings>() ?? throw new ArgumentNullException(sectionName, $"'{sectionName}' is null or empty in appSettings.json");

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
        
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
