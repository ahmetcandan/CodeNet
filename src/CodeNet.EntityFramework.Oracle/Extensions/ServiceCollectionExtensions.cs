﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Sql Server
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName">appSettings.json must contain ConnectionStrings:connectionName</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddOracle(this WebApplicationBuilder webBuilder, string connectionName)
    {
        webBuilder.AddOracle<DbContext>(connectionName);
        return webBuilder;
    }

    /// <summary>
    /// Add Npgsql
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="webBuilder"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddOracle<TDbContext>(this WebApplicationBuilder webBuilder, string connectionName) 
        where TDbContext : DbContext
    {
        webBuilder.Services.AddDbContext<TDbContext>(options => options.UseOracle(webBuilder.Configuration.GetConnectionString(connectionName)));
        return webBuilder;
    }
}
