using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CodeNet.Identity.DbContext;
using Microsoft.AspNetCore.Identity;
using CodeNet.Identity.Model;

namespace CodeNet.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Identity
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">appSettings.json must contain the sectionName main block. Json must be type IdentityConfig</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder webBuilder, string sqlConnectionName, string sectionName)
    {
        webBuilder.AddSqlServer<ApplicationDbContext>(sqlConnectionName);
        webBuilder.Services.Configure<IdentityConfig>(webBuilder.Configuration.GetSection(sectionName));
        webBuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return webBuilder;
    }
}
