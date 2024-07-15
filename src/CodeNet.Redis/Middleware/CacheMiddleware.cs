using CodeNet.Redis.Attributes;
using CodeNet.Redis.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
using CodeNet.Core.Extensions;
using System.Reflection;
using CodeNet.Core;

namespace CodeNet.Redis;

internal sealed class CacheMiddleware(RequestDelegate next, IDistributedCache distributedCache) : BaseMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        var methodInfo = GetMethodInfo(context);
        if (methodInfo?.GetCustomAttribute(typeof(CacheAttribute), true) is not CacheAttribute cacheAttribute)
        {
            await next(context);
            return;
        }

        var cacheState = context.Response.Headers.GetCacheState();

        string? key = null;
        if (cacheState.HasFlag(Core.Enums.CacheState.ClearCache))
        {
            key ??= await GetRequestKey(context, methodInfo);
            await distributedCache.RemoveAsync(key, context.RequestAborted);
        }

        if (cacheState.HasFlag(Core.Enums.CacheState.NoCache))
        {
            context.Response.Headers.SetResponseHeader(HeaderNames.CacheControl, Constant.NoCache);
            await next(context);
            return;
        }

        key ??= await GetRequestKey(context, methodInfo);
        var cacheValue = await distributedCache.GetStringAsync(key, context.RequestAborted);

        if (string.IsNullOrEmpty(cacheValue))
        {
            var response = await ReadResponseAsync(context, next, context.RequestAborted);
            if (context.Response.StatusCode is StatusCodes.Status200OK)
                await distributedCache.SetStringAsync(key, response, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheAttribute.Time)
                }, context.RequestAborted);
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.Headers.SetResponseHeader(HeaderNames.CacheControl, Constant.FromCache);
        await context.Response.WriteAsync(cacheValue, context.RequestAborted);
        return;
    }
}
