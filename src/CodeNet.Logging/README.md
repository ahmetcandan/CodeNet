## CodeNet.Logging

CodeNet.Logging is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Logging/) to install CodeNet.Logging

```bash
dotnet add package CodeNet.Logging
```

### Usage
program.cs
```csharp
using CodeNet.Logging.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogging(builder.Configuration.GetSection("Logging"));
//...

var app = builder.Build();
app.UseLogging();
//...
app.Run();
```