﻿using CodeNet.Core;
using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;

namespace CodeNet.Logging.Middleware;

public class LoggingMiddleware(RequestDelegate next) : BaseMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        var methodInfo = GetMethodInfo(context);
        var appLogger = context.RequestServices.GetRequiredService<IAppLogger>();

        appLogger.EntryLog(new
        {
            context.Request.Method,
            context.Request.Path,
            context.Request.Scheme,
            context.Request.RouteValues
        }, methodInfo!);

        var timer = new Stopwatch();
        timer.Start();

        await next(context);

        timer.Stop();
        var fromCache = context.Response.Headers[HeaderNames.CacheControl].ToString();
        appLogger.ExitLog(new
        {
            context.Response.ContentType,
            context.Response.StatusCode,
            CacheControl = string.IsNullOrEmpty(fromCache) ? Constant.NoCache : fromCache
        }, methodInfo!, timer.ElapsedMilliseconds);
    }
}
