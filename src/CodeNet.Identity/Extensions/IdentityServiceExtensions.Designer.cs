using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.DbContext;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Extensions;

public static partial class IdentityServiceExtensions
{
    private const string _codeNet = "CodeNet";

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
    public static IServiceCollection AddAuthorization<TUser, TRole, TKey>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, SecurityKeyType securityKeyType, IConfigurationSection authorizationSection, Action<IdentityOptionsBuilder> action = null)
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        switch (securityKeyType)
        {
            case SecurityKeyType.AsymmetricKey:
                var optionsAsymetric = authorizationSection.Get<IdentitySettingsWithAsymmetricKey>() ?? throw new ArgumentNullException($"'{authorizationSection.Path}' is null or empty in appSettings.json");
                return services.AddAuthorization<TUser, TRole, TKey>(optionsAction, optionsAsymetric, action);
            case SecurityKeyType.SymmetricKey:
                var optionsSymetric = authorizationSection.Get<IdentitySettingsWithSymmetricKey>() ?? throw new ArgumentNullException($"'{authorizationSection.Path}' is null or empty in appSettings.json");
                return services.AddAuthorization<TUser, TRole, TKey>(optionsAction, optionsSymetric, action);
            default:
                throw new NotImplementedException($"{nameof(SecurityKeyType)}: {securityKeyType}, not implemented.");
        }
    }

    public static IServiceCollection AddAuthorization<TUserManager, TRoleManager, TTokenManager>(this IServiceCollection services)
        where TUserManager : class, IIdentityUserManager
        where TRoleManager : class, IIdentityRoleManager
        where TTokenManager : class, IIdentityTokenManager
    {
        services.AddScoped<IIdentityUserManager, TUserManager>();
        services.AddScoped<IIdentityRoleManager, TRoleManager>();
        services.AddScoped<IIdentityTokenManager, TTokenManager>();
        return services;
    }

    /// <summary>
    /// Add Authorization
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    internal static IServiceCollection AddAuthorization<TUser, TRole, TKey>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, BaseIdentitySettings settings, Action<IdentityOptionsBuilder> action = null)
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        services.AddDbContext<CodeNetIdentityDbContext<TUser, TRole, TKey>>(optionsAction);
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
                AuthenticatorTokenProvider = settings.IdentityOptions?.Tokens?.AuthenticatorTokenProvider ?? _codeNet,
                EmailConfirmationTokenProvider = settings.IdentityOptions?.Tokens?.EmailConfirmationTokenProvider ?? _codeNet,
                PasswordResetTokenProvider = settings.IdentityOptions?.Tokens?.PasswordResetTokenProvider ?? _codeNet,
                AuthenticatorIssuer = settings.IdentityOptions?.Tokens?.AuthenticatorIssuer ?? _codeNet,
                ChangeEmailTokenProvider = settings.IdentityOptions?.Tokens?.ChangeEmailTokenProvider ?? _codeNet,
                ChangePhoneNumberTokenProvider = settings.IdentityOptions?.Tokens?.ChangePhoneNumberTokenProvider ?? _codeNet,
                ProviderMap = settings.IdentityOptions?.Tokens?.ProviderMap ?? []
            };
            c.Stores = new StoreOptions
            {
                MaxLengthForKeys = settings.IdentityOptions?.Stores?.MaxLengthForKeys ?? 0,
                ProtectPersonalData = settings.IdentityOptions?.Stores?.ProtectPersonalData ?? false,
                SchemaVersion = settings.IdentityOptions?.Stores?.SchemaVersion ?? IdentitySchemaVersions.Default
            };

        })
            .AddEntityFrameworkStores<CodeNetIdentityDbContext<TUser, TRole, TKey>>()
            .AddDefaultTokenProviders();
        services.AddScoped<IIdentityUserManager, IdentityUserManager<TUser, TRole, TKey>>();

        if (action is not null)
        {
            var builder = new IdentityOptionsBuilder(services);
            action(builder);
        }

        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        return services.AddScoped<IIdentityRoleManager, IdentityRoleManager<TRole, TKey>>();
    }

    /// <summary>
    /// Add Authorization Register With AsymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    internal static void AddAuthorizationRegisterWithAsymmetricKey<TUser, TRole, TKey>(this IServiceCollection services, IdentitySettingsWithAsymmetricKey settings)
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
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
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithAsymmetricKey<TUser, TRole, TKey>>();
    }

    /// <summary>
    /// Add Authorization Register With SymmetricKey
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    internal static void AddAuthorizationRegisterWithSymmetricKey<TUser, TRole, TKey>(this IServiceCollection services, IdentitySettingsWithSymmetricKey settings)
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
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
        services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithSymmetricKey<TUser, TRole, TKey>>();
    }
}
