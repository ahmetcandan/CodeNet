using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using Newtonsoft.Json;
using System.Text;

namespace NetCore.Container;

public class CacheHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IDistributedCache DistributedCache) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodBase = GetHandlerMethodInfo(LifetimeScope);
        ArgumentNullException.ThrowIfNull(methodBase);

        var cacheAttribute = methodBase?.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() as CacheAttribute;
        if (cacheAttribute is not null)
        {
            string key = $"{methodBase?.DeclaringType?.Assembly.GetName().Name}:{methodBase?.DeclaringType?.Name}:{RequestKey(request)}";

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
            if (cacheValue is not null)
            {
                cacheValue.FromCache = true;
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
        foreach (var prop in typeof(TRequest).GetProperties())
            stringBuilder.Append(prop.GetValue(request)?.ToString());

        return stringBuilder.ToString();
    }
}
