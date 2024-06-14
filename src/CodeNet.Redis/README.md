## CodeNet.Redis

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis.

```bash
dotnet add package CodeNet.Redis
```

### Usage
#### appSettings.json
```json
{
  "Redis": {
    "Hostname": "localhost",
    "Port": 6379
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
//for Distributed Cache
builder.AddRedisDistributedCache("Redis");

//for Distributed Lock
builder.AddRedisDistributedLock("Redis");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    //for Distributed Cache
    containerBuilder.RegisterModule<RedisDistributedCacheModule>();

    //for Distributed Lock
    containerBuilder.RegisterModule<RedisDistributedLockModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```