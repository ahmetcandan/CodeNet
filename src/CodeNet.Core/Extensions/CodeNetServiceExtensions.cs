using CodeNet.Core.Enums;
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
using System.Text;

namespace CodeNet.Core.Extensions;

public static class CodeNetServiceExtensions
{
    /// <summary>
    /// Use CodeNet
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static WebApplication UseCodeNet(this WebApplication app, IConfiguration configuration, string sectionName)
    {
        return app.UseCodeNet(configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Use CodeNet
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplication UseCodeNet(this WebApplication app, IConfigurationSection configurationSection)
    {
        var applicationSettings = configurationSection.Get<ApplicationSettings>() ?? throw new ArgumentNullException($"'{configurationSection.Path}' is null or empty in appSettings.json");

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

    /// <summary>
    /// Add CodeNet Configuration
    /// This method contains AddCodeNetContext.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddCodeNet(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        return services.AddCodeNet(configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add CodeNet Configuration
    /// This method contains AddCodeNetContext.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddCodeNet(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        Console.WriteLine(@"
   ___            _         _  _         _   
  / __|  ___   __| |  ___  | \| |  ___  | |_ 
 | (__  / _ \ / _` | / -_) | .` | / -_) |  _|
  \___| \___/ \__,_| \___| |_|\_| \___|  \__|
                                             ");

        var applicationSettings = configurationSection.Get<ApplicationSettings>() ?? throw new ArgumentNullException($"'{configurationSection.Path}' is null or empty in appSettings.json");

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
        services.AddHttpContextAccessor();
        return services.AddCodeNetContext();
    }

    /// <summary>
    /// Add CodeNetContext
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCodeNetContext(this IServiceCollection services)
    {
        return services.AddScoped<ICodeNetContext, CodeNetContext>();
    }

    /// <summary>
    /// Add Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthentication(this IServiceCollection services, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName)
    {
        return services.AddAuthentication(securityKeyType, configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IServiceCollection AddAuthentication(this IServiceCollection services, SecurityKeyType securityKeyType, IConfigurationSection identitySection)
    {
        return securityKeyType switch
        {
            SecurityKeyType.AsymmetricKey => services.AddAuthenticationWithAsymmetricKey(identitySection),
            SecurityKeyType.SymmetricKey => services.AddAuthenticationWithSymmetricKey(identitySection),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Add Authentication With Asymmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static IServiceCollection AddAuthenticationWithAsymmetricKey(this IServiceCollection services, IConfigurationSection identitySection)
    {
        var authenticationSettings = identitySection.Get<AuthenticationSettingsWithAsymmetricKey>() ?? throw new ArgumentNullException($"'{identitySection.Path}' is null or empty in appSettings.json");
        var rsa = AsymmetricKeyEncryption.CreateRSA(authenticationSettings.PublicKeyPath);
        services.AddAuthentication(options =>
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

        return services;
    }

    /// <summary>
    /// Add Authentication With Symmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="applicationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static IServiceCollection AddAuthenticationWithSymmetricKey(this IServiceCollection services, IConfigurationSection applicationSection)
    {
        var authenticationSettings = applicationSection.Get<AuthenticationSettingsWithSymmetricKey>() ?? throw new ArgumentNullException($"'{applicationSection.Path}' is null or empty in appSettings.json");
        services.AddAuthentication(options =>
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
                ValidAudience = authenticationSettings.ValidAudience,
                ValidIssuer = authenticationSettings.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.IssuerSigningKey))
            };
        });

        return services;
    }
}
