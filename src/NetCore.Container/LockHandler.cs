using Autofac;
using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using RedLockNet;

namespace NetCore.Container;

public class LockHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedLockFactory LockFactory) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(LifetimeScope);

        if (methodBase?.GetCustomAttributes(typeof(LockAttribute), true).FirstOrDefault() is not LockAttribute distributedLockAttribute)
            return await next();

        using var redLock = await LockFactory.CreateLockAsync(GetKey(methodBase, request), TimeSpan.FromSeconds(distributedLockAttribute.ExpiryTime));
        if (redLock.IsAcquired)
            return await next();
        else
            return new TResponse
            {
                IsSuccessfull = false,
                Message = "Distributed Lock Fail !",
                MessageCode = "012",
                FromCache = false
            };
    }
}
