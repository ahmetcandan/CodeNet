using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
using CodeNet.Core.Extensions;
using System.Reflection;
using CodeNet.Core;
using CodeNet.Core.Settings;

namespace CodeNet.Redis;

internal sealed class CacheMiddleware(RequestDelegate next, IDistributedCache distributedCache) : BaseMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        var methodInfo = GetMethodInfo(context);
        if (methodInfo?.GetCustomAttribute(typeof(CacheAttribute), true) is not CacheAttribute cacheAttribute)
        {
            await next(context);
        }
        else
        {
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
            }
            else
            {
                key ??= await GetRequestKey(context, methodInfo);
                var cacheValue = await distributedCache.GetStringAsync(key, context.RequestAborted);

                if (string.IsNullOrEmpty(cacheValue))
                {
                    var response = await ReadResponseAsync(context, next);
                    if (context.Response.StatusCode is StatusCodes.Status200OK)
                        await distributedCache.SetStringAsync(key, response, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheAttribute.Time)
                        }, context.RequestAborted);
                }
                else
                {
                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
                    context.Response.Headers.SetResponseHeader(HeaderNames.CacheControl, Constant.FromCache);
                    await context.Response.WriteAsync(cacheValue, context.RequestAborted);
                }
            }
        }
    }
}
