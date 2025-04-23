﻿using CodeNet.Core;
using CodeNet.Redis.Attributes;
using CodeNet.Redis.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;

namespace CodeNet.Redis;

internal sealed class LockMiddleware(RequestDelegate next) : BaseMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        var methodInfo = GetMethodInfo(context);

        if (methodInfo?.GetCustomAttributes(typeof(LockAttribute), true).FirstOrDefault() is not LockAttribute distributedLockAttribute)
        {
            await next(context);
            return;
        }

        var key = await GetRequestKey(context, methodInfo);
        var lockFactory = context.RequestServices.GetRequiredService<IDistributedLockFactory>();
        using var redLock = await lockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(distributedLockAttribute.ExpiryTime));

        if (!redLock.IsAcquired)
            throw new RedisException("RDS0012", "Distributed Lock Fail !");

        await next(context);
        await redLock.DisposeAsync();

        return;
    }
}
