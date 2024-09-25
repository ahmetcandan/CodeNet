## CodeNet.HealthCheck.Elasticsearch

CodeNet.HealthCheck.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.Elasticsearch/) to install CodeNet.HealthCheck.Elasticsearch

```bash
dotnet add package CodeNet.HealthCheck.Elasticsearch
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Elasticsearch.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks(options =>
    {
        options.AddElasticsearchHealthCheck();
    });
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```