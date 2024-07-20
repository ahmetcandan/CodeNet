using CodeNet.Core.Enums;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Manager;
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
        return securityKeyType switch
        {
            SecurityKeyType.AsymmetricKey => services.AddAuthorizationWithAsymmetricKey(optionsAction, authorizationSection),
            SecurityKeyType.SymmetricKey => services.AddAuthorizationWithSymmetricKey(optionsAction, authorizationSection),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Add Identity with other Database
    /// Asymmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    internal static IServiceCollection AddAuthorizationWithAsymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
    {
        services.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        services.Configure<IdentitySettingsWithAsymmetricKey>(identitySection);
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithAsymmetricKey>();
        services.AddScoped<IIdentityUserManager, IdentityUserManager>();
        return services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
    }

    /// <summary>
    /// Add Identity with other Database
    /// Symmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    internal static IServiceCollection AddAuthorizationWithSymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
    {
        services.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        services.Configure<IdentitySettingsWithSymmetricKey>(identitySection);
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithSymmetricKey>();
        services.AddScoped<IIdentityUserManager, IdentityUserManager>();
        return services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
    }
}
