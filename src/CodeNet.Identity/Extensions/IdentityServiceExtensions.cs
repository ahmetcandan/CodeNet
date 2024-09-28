using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Extensions;

public static class IdentityServiceExtensions
{
    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName)
    {
        return services.AddAuthorization(optionsAction, securityKeyType, configuration.GetSection(sectionName));
    }

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="authorizationSection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection)
    {
        services.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        switch (securityKeyType)
        {
            case SecurityKeyType.AsymmetricKey:
                services.AddAuthorizationWithAsymmetricKey(optionsAction, authorizationSection);
                break;
            case SecurityKeyType.SymmetricKey:
                services.AddAuthorizationWithSymmetricKey(optionsAction, authorizationSection);
                break;
            default:
                throw new NotImplementedException($"{nameof(SecurityKeyType)}: {securityKeyType}, not implemented.");
        }
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IIdentityUserManager, IdentityUserManager>();
        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        return services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
    }

    /// <summary>
    /// Add Identity with other Database
    /// Asymmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    internal static void AddAuthorizationWithAsymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
    {
        services.Configure<IdentitySettingsWithAsymmetricKey>(identitySection);
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithAsymmetricKey>();
    }

    /// <summary>
    /// Add Identity with other Database
    /// Symmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    internal static void AddAuthorizationWithSymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
    {
        services.Configure<IdentitySettingsWithSymmetricKey>(identitySection);
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithSymmetricKey>();
    }
}
