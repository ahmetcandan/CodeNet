## CodeNet.Redis

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis.

```bash
dotnet add package CodeNet.Redis
```

### Usage
appSettings.json
```json
{
  "Redis": {
    "Hostname": "localhost",
    "Port": 6379
  }
}
```
program.cs
```csharp
using CodeNet.Container.Extensions;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.Redis.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    //for cache
    containerBuilder.AddModule<RedisDistributedCacheModule>();

    //for lock
    containerBuilder.AddModule<RedisDistributedLockModule>();
});
builder
    .AddRedisDistributedCache("Redis")
    .AddRedisDistributedLock("Redis");
//...

var app = builder.Build();
//...
app.Run();
```
#### Usage Lock
```csharp  
using MediatR;
using RedLockNet;

namespace ExampleApi.Handler;

public class TestRequestHandler(IDistributedLockFactory LockFactory) : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        using var redLock = await LockFactory.CreateLockAsync("LOCK_KEY", TimeSpan.FromSeconds(3));
        if (!redLock.IsAcquired)
            throw new SynchronizationLockException();

        //Process...
    }
}
```
Or
```csharp  
using CodeNet.Redis.Attributes;
using MediatR;

namespace ExampleApi.Handler;

public class TestRequestHandler() : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    [Lock(ExpiryTime = 3)]
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        //Process...
    }
}
```
#### Usage Cache
```csharp  
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ExampleApi.Handler;

public class TestRequestHandler(IDistributedLockFactory LockFactory) : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    private const string CACHE_KEY = "KEY";
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        var cacheJsonValue = await DistributedCache.GetStringAsync(CACHE_KEY, cancellationToken);
        if (string.IsNullOrEmpty(cacheJsonValue))
        {
            //Process...
            var response = ...
            await DistributedCache.SetStringAsync(CACHE_KEY, JsonConvert.SerializeObject(response), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            }, cancellationToken);
            return response;
        }
        return JsonConvert.DeserializeObject<ResponseBase<TestResponse>>(cacheJsonValue);
    }
}
```
Or
```csharp  
using CodeNet.Redis.Attributes;
using MediatR;

namespace ExampleApi.Handler;

public class TestRequestHandler() : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    [Cache(Time = 60)]
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        //Process...
    }
}
```
