using CodeNet.Core.Context;
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
        SecurityKey? securityKey;
        AuthenticationSettings? authenticationSettings;
        switch (securityKeyType)
        {
            case SecurityKeyType.AsymmetricKey:
                var asymmetricKeysettings = identitySection.Get<AuthenticationSettingsWithAsymmetricKey>() ?? throw new ArgumentNullException($"'{identitySection.Path}' is null or empty in appSettings.json");
                securityKey = GetAsymmetricKey(asymmetricKeysettings);
                authenticationSettings = asymmetricKeysettings;
                break;
            case SecurityKeyType.SymmetricKey:
                var symmetricKeysettings = identitySection.Get<AuthenticationSettingsWithSymmetricKey>() ?? throw new ArgumentNullException($"'{identitySection.Path}' is null or empty in appSettings.json");
                securityKey = GetSymmetricKey(symmetricKeysettings);
                authenticationSettings = symmetricKeysettings;
                break;
            default:
                throw new NotImplementedException();
        }
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
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = authenticationSettings.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = authenticationSettings.ValidAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return this;
    }

    private static RsaSecurityKey GetAsymmetricKey(AuthenticationSettingsWithAsymmetricKey authenticationSettings) => new(AsymmetricKeyEncryption.CreateRSA(authenticationSettings.PublicKeyPath));

    private static SymmetricSecurityKey GetSymmetricKey(AuthenticationSettingsWithSymmetricKey authenticationSettings) => new(Encoding.UTF8.GetBytes(authenticationSettings.IssuerSigningKey));

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
