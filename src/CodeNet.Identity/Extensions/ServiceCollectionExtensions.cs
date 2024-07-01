using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Identity with SqlServer
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type IdentityConfig</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder webBuilder, string connectionName, string sectionName)
    {
        webBuilder.AddDbContext<CodeNetIdentityDbContext>(connectionName);
        webBuilder.Services.Configure<IdentityConfig>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();
        return webBuilder;
    }

    /// <summary>
    /// Add Identity with other Database
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="AddOtherDB"></param>
    /// <param name="connectionName"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder webBuilder, Action<DbContextOptionsBuilder> optionsAction, string sectionName)
    {
        webBuilder.AddDbContext<CodeNetIdentityDbContext>(optionsAction);
        webBuilder.Services.Configure<IdentityConfig>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CodeNetIdentityDbContext>()
            .AddDefaultTokenProviders();
        return webBuilder;
    }
}
