using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Newtonsoft.Json;
using CodeNet.Logging;
using System.Reflection;
using Microsoft.Net.Http.Headers;
using CodeNet.Redis.Settings;

namespace CodeNet.Redis.Handler;

internal class CacheHandler<TRequest, TResponse>(ILifetimeScope lifetimeScope, IDistributedCache distributedCache, ICodeNetHttpContext httpContext, IAppLogger appLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(lifetimeScope);

        var cacheState = httpContext.CacheState;

        if (methodBase?.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() is not CacheAttribute cacheAttribute)
            return await next();

        if (cacheState.HasFlag(Core.Enums.CacheState.NoCache))
        {
            httpContext.SetResponseHeader(HeaderNames.CacheControl, Constant.NoCache);
            return await next();
        }

        var key = GetKey(methodBase, request);

        if (cacheState.HasFlag(Core.Enums.CacheState.ClearCache))
            await distributedCache.RemoveAsync(key, cancellationToken);

        var cacheJsonValue = await distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cacheJsonValue))
        {
            var response = await next();
            await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(response), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheAttribute.Time)
            }, cancellationToken);
            return response;
        }

        var cacheValue = JsonConvert.DeserializeObject<TResponse>(cacheJsonValue);
        appLogger.TraceLog(cacheJsonValue, MethodBase.GetCurrentMethod());
        cacheValue!.FromCache = true;
        httpContext.SetResponseHeader(HeaderNames.CacheControl, Constant.FromCache);
        return cacheValue;
    }
}
