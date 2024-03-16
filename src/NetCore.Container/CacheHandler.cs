using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using Newtonsoft.Json;
using ServiceStack;
using System.Text;

namespace NetCore.Container;

public class CacheHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedCache DistributedCache) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodInfo = GetHandlerMethodInfo(LifetimeScope);
        ArgumentNullException.ThrowIfNull(methodInfo);

        var attributes = methodInfo!.GetCustomAttributes(typeof(CacheAttribute), true);
        if (attributes?.Length > 0)
        {
            var cacheAttribute = (CacheAttribute)attributes[0];
            string key = $"{methodInfo?.DeclaringType?.Assembly.GetName().Name}:{methodInfo?.DeclaringType?.Name}:{RequestKey(request)}";

            var cacheJsonValue = await DistributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cacheJsonValue))
            {
                var response = await next();
                await DistributedCache.SetStringAsync(key, JsonConvert.SerializeObject(response), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheAttribute.Time) });
                return response;
            }

            var cacheValue = JsonConvert.DeserializeObject<TResponse>(cacheJsonValue);
            if (cacheValue is not null)
            {
                cacheValue.HasCacheData = true;
                return cacheValue;
            }
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
