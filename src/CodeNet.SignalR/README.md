## CodeNet.SignalR

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.SignalR/) to install CodeNet.Redis

```bash
dotnet add package CodeNet.SignalR
```

### Usage
program.cs
```csharp
using CodeNet.Container.Extensions;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.Redis.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalRNotification();
//...

var app = builder.Build();
app.UseSignalR<THub>("/tst");
//...
app.Run();
```
