﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Sql Server
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSqlServer(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddSqlServer<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>    
    /// Add Sql Server
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSqlServer<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) where TDbContext : DbContext
    {
        webBuilder.Services.AddDbContext<TDbContext>(options => options.UseSqlServer(webBuilder.Configuration.GetConnectionString(connectionName)));
        return webBuilder;
    }

}
