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

        var request = await GetRequest(context);
        appLogger.EntryLog(request, methodInfo!);

        var timer = new Stopwatch();
        timer.Start();

        var response = await ReadResponseAsync(context, next, context.RequestAborted);

        timer.Stop();
        appLogger.ExitLog(response, methodInfo!, timer.ElapsedMilliseconds);
    }
}
