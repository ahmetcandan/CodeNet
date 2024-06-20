﻿using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Newtonsoft.Json;

namespace CodeNet.Redis.Handler;

internal class CacheHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedCache DistributedCache) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(LifetimeScope);

        if (methodBase?.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() is not CacheAttribute cacheAttribute)
            return await next();

        var key = GetKey(methodBase, request);
        var cacheJsonValue = await DistributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cacheJsonValue))
        {
            var response = await next();
            await DistributedCache.SetStringAsync(key, JsonConvert.SerializeObject(response), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheAttribute.Time)
            }, cancellationToken);
            return response;
        }

        var cacheValue = JsonConvert.DeserializeObject<TResponse>(cacheJsonValue);
        cacheValue!.FromCache = true;
        return cacheValue;
    }
}
