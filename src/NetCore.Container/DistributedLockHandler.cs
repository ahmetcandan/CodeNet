using Autofac;
using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using RedLockNet;

namespace NetCore.Container;

public class DistributedLockHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedLockFactory LockFactory) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(LifetimeScope);

        var distributedLockAttribute = methodBase?.GetCustomAttributes(typeof(DistributedLockAttribute), true).FirstOrDefault() as DistributedLockAttribute;
        if (distributedLockAttribute is not null)
        {
            string key = $"{methodBase?.DeclaringType?.Assembly.GetName().Name}:{methodBase?.DeclaringType?.Name}:{RequestKey(request)}";

            using var redLock = await LockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(distributedLockAttribute.ExpiryTime));
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

        return await next();
    }
}
