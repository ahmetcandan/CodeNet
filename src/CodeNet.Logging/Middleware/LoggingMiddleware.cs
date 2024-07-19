using CodeNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        appLogger.ExitLog(new
        {
            context.Response.ContentType,
            context.Response.StatusCode
        }, methodInfo!, timer.ElapsedMilliseconds);
    }
}
