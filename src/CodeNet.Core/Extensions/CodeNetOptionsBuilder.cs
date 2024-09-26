using CodeNet.Core.Enums;
using CodeNet.Core.Security;
using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CodeNet.Core.Extensions;

public class CodeNetOptionsBuilder(IServiceCollection services)
{
    /// <summary>
    /// Add Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public CodeNetOptionsBuilder AddAuthentication(SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName)
    {
        return AddAuthentication(securityKeyType, configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="securityKeyType"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public CodeNetOptionsBuilder AddAuthentication(SecurityKeyType securityKeyType, IConfigurationSection identitySection)
    {
        return securityKeyType switch
        {
            SecurityKeyType.AsymmetricKey => AddAuthenticationWithAsymmetricKey(identitySection),
            SecurityKeyType.SymmetricKey => AddAuthenticationWithSymmetricKey(identitySection),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Add Authentication With Asymmetric Key
    /// </summary>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private CodeNetOptionsBuilder AddAuthenticationWithAsymmetricKey(IConfigurationSection identitySection)
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

        return this;
    }

    /// <summary>
    /// Add Authentication With Symmetric Key
    /// </summary>
    /// <param name="applicationSection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private CodeNetOptionsBuilder AddAuthenticationWithSymmetricKey(IConfigurationSection applicationSection)
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

        return this;
    }

    /// <summary>
    /// Add CodeNetContext
    /// </summary>
    /// <returns></returns>
    public CodeNetOptionsBuilder AddCodeNetContext()
    {
        if (!services.Any(c => c.ServiceType.Equals(typeof(ICodeNetContext))))
            services.AddScoped<ICodeNetContext, CodeNetContext>();

        return this;
    }
}
