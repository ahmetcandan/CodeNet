using Autofac;
using MediatR;
using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using RedLockNet;

namespace CodeNet.Redis.Handler;

public class LockHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedLockFactory LockFactory) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(LifetimeScope);

        if (methodBase?.GetCustomAttributes(typeof(LockAttribute), true).FirstOrDefault() is not LockAttribute distributedLockAttribute)
            return await next();

        using var redLock = await LockFactory.CreateLockAsync(GetKey(methodBase, request), TimeSpan.FromSeconds(distributedLockAttribute.ExpiryTime));
        return redLock.IsAcquired
            ? await next()
            : new TResponse
            {
                IsSuccessfull = false,
                Message = "Distributed Lock Fail !",
                MessageCode = "012",
                FromCache = false
            };
    }
}
