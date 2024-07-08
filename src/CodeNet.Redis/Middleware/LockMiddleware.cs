using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Http;
using CodeNet.Core;
using RedLockNet;
using CodeNet.Redis.Exception;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Redis;

public class LockMiddleware(RequestDelegate next) : BaseMiddleware
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
        return;
    }
}
