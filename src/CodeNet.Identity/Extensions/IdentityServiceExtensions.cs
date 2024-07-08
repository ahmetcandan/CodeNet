using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CodeNet.Identity.Manager;
using Microsoft.Extensions.Configuration;

namespace CodeNet.Identity.Extensions;

public static class IdentityServiceExtensions
{
    /// <summary>
    /// Add Identity with SqlServer
    /// Asymmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityWithAsymmetricKey(this IServiceCollection services, string connectionString, IConfigurationSection identitySection)
    {
        return services.AddIdentityWithAsymmetricKey(builder => builder.UseSqlServer(connectionString), identitySection);
    }

    /// <summary>
    /// Add Identity with other Database
    /// Asymmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityWithAsymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
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
    /// Add Identity with SqlServer
    /// Symmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityWithSymmetricKey(this IServiceCollection services, string connectionString, IConfigurationSection identitySection)
    {
        return services.AddIdentityWithSymmetricKey(builder => builder.UseSqlServer(connectionString), identitySection);
    }

    /// <summary>
    /// Add Identity with other Database
    /// Symmetric Key
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityWithSymmetricKey(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, IConfigurationSection identitySection)
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
