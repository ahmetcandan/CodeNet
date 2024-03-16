using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using ServiceStack;
using System.Text;

namespace NetCore.Container;

public class CacheHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, ICacheRepository CacheRepository) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodInfo = GetHandlerMethodInfo(LifetimeScope);
        methodInfo.ThrowIfNull();

        var attributes = methodInfo!.GetCustomAttributes(typeof(CacheAttribute), true);
        if (attributes?.Length > 0)
        {
            var cacheAttribute = (CacheAttribute)attributes[0];
            string key = $"{methodInfo?.DeclaringType?.Assembly.GetName().Name}:{methodInfo?.DeclaringType?.Name}:{RequestKey(request)}";

            var cacheValue = CacheRepository.GetCache<TResponse>(key);
            if (cacheValue is null)
            {
                var response = await next();
                CacheRepository.SetCache(key, response, cacheAttribute.Time);
                return response;
            }

            cacheValue.HasCacheData = true;
            return cacheValue;
        }

        return await next();
    }

    private static string RequestKey(TRequest request) 
    {
        if (request is null)
            return string.Empty;

        var stringBuilder = new StringBuilder();
        foreach (var prop in typeof(TRequest).Properties())
            stringBuilder.Append(prop.GetValue(request)?.ToString());

        return stringBuilder.ToString();
    }
}
