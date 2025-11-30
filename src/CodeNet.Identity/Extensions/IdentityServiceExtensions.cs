using CodeNet.Core.Enums;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Extensions;

public static partial class IdentityServiceExtensions
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
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName, Action<IdentityOptionsBuilder>? action = null)
        => services.AddAuthorization<IdentityUser>(optionsAction, securityKeyType, configuration, sectionName, action);

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        => services.AddAuthorization<TUser, IdentityRole>(optionsAction, securityKeyType, configuration, sectionName, action);

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        => services.AddAuthorization<TUser, TRole, string>(optionsAction, securityKeyType, configuration, sectionName, action);

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="configuration"></param>
    /// <param name="sectionName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole, TKey>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
        => services.AddAuthorization<TUser, TRole, TKey>(optionsAction, securityKeyType, configuration.GetSection(sectionName), action);

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
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder>? action = null)
        => services.AddAuthorization<IdentityUser>(optionsAction, securityKeyType, authorizationSection, action);

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="authorizationSection"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        => services.AddAuthorization<TUser, IdentityRole>(optionsAction, securityKeyType, authorizationSection, action);

    /// <summary>
    /// Add Authorization
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be IdentitySettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be IdentitySettingsWithSymmetricKey.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="securityKeyType"></param>
    /// <param name="authorizationSection"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        => services.AddAuthorization<TUser, TRole, string>(optionsAction, securityKeyType, authorizationSection, action);

    /// <summary>
    /// Add Authorization With AsymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithAsymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        => services.AddAuthorization<IdentityUser>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With AsymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithAsymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        => services.AddAuthorization<TUser, IdentityRole>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With AsymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithAsymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        => services.AddAuthorization<TUser, TRole, string>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With AsymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole, TKey>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithAsymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        services.AddAuthorizationRegisterWithAsymmetricKey<TUser, TRole, TKey>(settings);
        return services.AddAuthorization<TUser, TRole, TKey>(optionsAction, (BaseIdentitySettings)settings, action);
    }

    /// <summary>
    /// Add Authorization With SymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithSymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        => services.AddAuthorization<IdentityUser>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With SymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithSymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        => services.AddAuthorization<TUser, IdentityRole>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With SymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithSymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        => services.AddAuthorization<TUser, TRole, string>(optionsAction, settings, action);

    /// <summary>
    /// Add Authorization With SymmetricKey
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization<TUser, TRole, TKey>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithSymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        services.AddAuthorizationRegisterWithSymmetricKey<TUser, TRole, TKey>(settings);
        return services.AddAuthorization<TUser, TRole, TKey>(optionsAction, (BaseIdentitySettings)settings, action);
    }
}
