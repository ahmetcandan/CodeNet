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
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfiguration configuration, string sectionName, Action<IdentityOptionsBuilder>? action = null)
    {
        return services.AddAuthorization<ApplicationUser>(optionsAction, securityKeyType, configuration, sectionName, action);
    }

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
        where TUser : ApplicationUser, new()
    {
        return services.AddAuthorization<TUser>(optionsAction, securityKeyType, configuration.GetSection(sectionName), action);
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
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder>? action = null)
    {
        return services.AddAuthorization<ApplicationUser>(optionsAction, securityKeyType, authorizationSection, action);
    }

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
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder>? action = null)
        where TUser : ApplicationUser, new()
    {
        switch (securityKeyType)
        {
            case SecurityKeyType.AsymmetricKey:
                var optionsAsymetric = authorizationSection.Get<IdentitySettingsWithAsymmetricKey>() ?? throw new ArgumentNullException($"'{authorizationSection.Path}' is null or empty in appSettings.json");
                return services.AddAuthorization<TUser>(optionsAction, optionsAsymetric, action);
            case SecurityKeyType.SymmetricKey:
                var optionsSymetric = authorizationSection.Get<IdentitySettingsWithSymmetricKey>() ?? throw new ArgumentNullException($"'{authorizationSection.Path}' is null or empty in appSettings.json");
                return services.AddAuthorization<TUser>(optionsAction, optionsSymetric, action);
            default:
                throw new NotImplementedException($"{nameof(SecurityKeyType)}: {securityKeyType}, not implemented.");
        }
    }

    /// <summary>
    /// Add Authorization With AsymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithAsymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
    {
        return services.AddAuthorization<ApplicationUser>(optionsAction, settings, action);
    }

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
        where TUser : ApplicationUser, new()
    {
        services.AddAuthorizationRegisterWithAsymmetricKey<TUser>(settings);
        return services.AddAuthorization<TUser>(optionsAction, (BaseIdentitySettings)settings, action);
    }

    /// <summary>
    /// Add Authorization With SymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IdentitySettingsWithSymmetricKey settings, Action<IdentityOptionsBuilder>? action = null)
    {
        return services.AddAuthorization<ApplicationUser>(optionsAction, settings, action);
    }

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
        where TUser : ApplicationUser, new()
    {
        services.AddAuthorizationRegisterWithSymmetricKey<TUser>(settings);
        return services.AddAuthorization<TUser>(optionsAction, (BaseIdentitySettings)settings, action);
    }

    /// <summary>
    /// Add Authorization
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    internal static IServiceCollection AddAuthorization<TUser>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, BaseIdentitySettings settings, Action<IdentityOptionsBuilder>? action = null)
        where TUser : ApplicationUser, new()
    {
        services.AddDbContext<CodeNetIdentityDbContext<TUser>>(optionsAction);
        services.AddIdentity<TUser, IdentityRole>(c =>
        {
            c.ClaimsIdentity = new ClaimsIdentityOptions
            {
                UserIdClaimType = settings.IdentityOptions?.ClaimsIdentity?.UserIdClaimType ?? "id",
                UserNameClaimType = settings.IdentityOptions?.ClaimsIdentity?.UserNameClaimType ?? "name"
            };
            c.User = new UserOptions
            {
                AllowedUserNameCharacters = settings.IdentityOptions?.User?.AllowedUserNameCharacters ?? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
            };
            c.Password = new PasswordOptions
            {
                RequireDigit = settings.IdentityOptions?.Password?.RequireDigit ?? true,
                RequiredLength = settings.IdentityOptions?.Password?.RequiredLength ?? 6,
                RequireLowercase = settings.IdentityOptions?.Password?.RequireLowercase ?? true,
                RequireNonAlphanumeric = settings.IdentityOptions?.Password?.RequireNonAlphanumeric ?? true,
                RequireUppercase = settings.IdentityOptions?.Password?.RequireUppercase ?? true,
                RequiredUniqueChars = settings.IdentityOptions?.Password?.RequiredUniqueChars ?? 1
            };
            c.Lockout = new LockoutOptions
            {
                DefaultLockoutTimeSpan = settings.IdentityOptions?.Lockout?.DefaultLockoutTimeSpan ?? TimeSpan.FromMinutes(5),
                MaxFailedAccessAttempts = settings.IdentityOptions?.Lockout?.MaxFailedAccessAttempts ?? 5,
                AllowedForNewUsers = settings.IdentityOptions?.Lockout?.AllowedForNewUsers ?? true
            };
            c.SignIn = new SignInOptions
            {
                RequireConfirmedAccount = settings.IdentityOptions?.SignIn?.RequireConfirmedAccount ?? false,
                RequireConfirmedEmail = settings.IdentityOptions?.SignIn?.RequireConfirmedEmail ?? false,
                RequireConfirmedPhoneNumber = settings.IdentityOptions?.SignIn?.RequireConfirmedPhoneNumber ?? false
            };
            c.Tokens = new TokenOptions
            {
                AuthenticatorTokenProvider = settings.IdentityOptions?.Tokens?.AuthenticatorTokenProvider ?? "CodeNet",
                EmailConfirmationTokenProvider = settings.IdentityOptions?.Tokens?.EmailConfirmationTokenProvider ?? "CodeNet",
                PasswordResetTokenProvider = settings.IdentityOptions?.Tokens?.PasswordResetTokenProvider ?? "CodeNet",
                AuthenticatorIssuer = settings.IdentityOptions?.Tokens?.AuthenticatorIssuer ?? "CodeNet",
                ChangeEmailTokenProvider = settings.IdentityOptions?.Tokens?.ChangeEmailTokenProvider ?? "CodeNet",
                ChangePhoneNumberTokenProvider = settings.IdentityOptions?.Tokens?.ChangePhoneNumberTokenProvider ?? "CodeNet",
                ProviderMap = settings.IdentityOptions?.Tokens?.ProviderMap ?? []
            };
            c.Stores = new StoreOptions
            {
                MaxLengthForKeys = settings.IdentityOptions?.Stores?.MaxLengthForKeys ?? 0,
                ProtectPersonalData = settings.IdentityOptions?.Stores?.ProtectPersonalData ?? false,
                SchemaVersion = settings.IdentityOptions?.Stores?.SchemaVersion ?? IdentitySchemaVersions.Default
            };

        })
            .AddEntityFrameworkStores<CodeNetIdentityDbContext<TUser>>()
            .AddDefaultTokenProviders();
        services.AddScoped<IIdentityUserManager, IdentityUserManager<TUser>>();

        if (action is not null)
        {
            var builder = new IdentityOptionsBuilder(services);
            action(builder);
        }

        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        return services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
    }

    /// <summary>
    /// Add Authorization Register With AsymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    internal static void AddAuthorizationRegisterWithAsymmetricKey<TUser>(this IServiceCollection services, IdentitySettingsWithAsymmetricKey settings)
        where TUser : ApplicationUser
    {
        services.Configure<IdentitySettingsWithAsymmetricKey>(c =>
        {
            c.ValidIssuer = settings.ValidIssuer;
            c.ValidAudience = settings.ValidAudience;
            c.RefreshTokenExpiryTime = settings.RefreshTokenExpiryTime;
            c.ExpiryTime = settings.ExpiryTime;
            c.PrivateKeyPath = settings.PrivateKeyPath;
            c.SecurityAlgorithm = settings.SecurityAlgorithm;
        });
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithAsymmetricKey<TUser>>();
    }


    /// <summary>
    /// Add Authorization Register With SymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    internal static void AddAuthorizationRegisterWithSymmetricKey<TUser>(this IServiceCollection services, IdentitySettingsWithSymmetricKey settings)
        where TUser : ApplicationUser
    {
        services.Configure<IdentitySettingsWithSymmetricKey>(c =>
        {
            c.ValidIssuer = settings.ValidIssuer;
            c.ValidAudience = settings.ValidAudience;
            c.RefreshTokenExpiryTime = settings.RefreshTokenExpiryTime;
            c.ExpiryTime = settings.ExpiryTime;
            c.IssuerSigningKey = settings.IssuerSigningKey;
            c.SecurityAlgorithm = settings.SecurityAlgorithm;
        });
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithSymmetricKey<TUser>>();
    }
}
