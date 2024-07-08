using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using CodeNet.Identity.Manager;

namespace CodeNet.Identity.Extensions;

public static class IdentityServiceExtensions
{
    /// <summary>
    /// Add Identity with SqlServer
    /// Asymmetric Key
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type IdentityConfig</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddIdentityWithAsymmetricKey(this IHostApplicationBuilder webBuilder, string connectionName, string sectionName)
    {
        return webBuilder.AddIdentityWithAsymmetricKey(builder => builder.UseSqlServer(webBuilder.Configuration, connectionName), sectionName);
    }

    /// <summary>
    /// Add Identity with other Database
    /// Asymmetric Key
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="AddOtherDB"></param>
    /// <param name="connectionName"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddIdentityWithAsymmetricKey(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string sectionName)
    {
        webBuilder.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        webBuilder.Services.Configure<IdentitySettingsWithAsymmetricKey>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();

        webBuilder.Services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithAsymmetricKey>();
        webBuilder.Services.AddScoped<IIdentityUserManager, IdentityUserManager>();
        webBuilder.Services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
        return webBuilder;
    }

    /// <summary>
    /// Add Identity with SqlServer
    /// Symmetric Key
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type IdentityConfig</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddIdentityWithSymmetricKey(this IHostApplicationBuilder webBuilder, string connectionName, string sectionName)
    {
        return webBuilder.AddIdentityWithSymmetricKey(builder => builder.UseSqlServer(webBuilder.Configuration, connectionName), sectionName);
    }

    /// <summary>
    /// Add Identity with other Database
    /// Symmetric Key
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="AddOtherDB"></param>
    /// <param name="connectionName"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddIdentityWithSymmetricKey(this IHostApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string sectionName)
    {
        webBuilder.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        webBuilder.Services.Configure<IdentitySettingsWithSymmetricKey>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();

        webBuilder.Services.AddScoped<IIdentityTokenManager, IdentityTokenManagerWithSymmetricKey>();
        webBuilder.Services.AddScoped<IIdentityUserManager, IdentityUserManager>();
        webBuilder.Services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
        return webBuilder;
    }
}
