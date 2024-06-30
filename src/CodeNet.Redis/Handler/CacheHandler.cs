using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Newtonsoft.Json;
using CodeNet.Logging;
using System.Reflection;

namespace CodeNet.Redis.Handler;

internal class CacheHandler<TRequest, TResponse>(ILifetimeScope lifetimeScope, IDistributedCache distributedCache, IIdentityContext identityContext, IAppLogger appLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string? key = null;
        var methodBase = GetHandlerMethodInfo(lifetimeScope);

        var cacheState = identityContext.CacheState;

        if (methodBase?.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() is not CacheAttribute cacheAttribute)
            return await next();

        if (cacheState.HasFlag(Core.Enums.CacheState.NoCache))
            return await next();

         key = GetKey(methodBase, request);
        var cacheJsonValue = await distributedCache.GetStringAsync(key, cancellationToken);

        if (cacheState.HasFlag(Core.Enums.CacheState.ClearCache) && cacheJsonValue is not null)
            await distributedCache.RemoveAsync(key, cancellationToken);

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
        return cacheValue;
    }
}
