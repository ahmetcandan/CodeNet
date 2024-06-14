## CodeNet.Elasticsearch

CodeNet.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Elasticsearch/) to install CodeNet.Elasticsearch.

```bash
dotnet add package CodeNet.Elasticsearch
```

### Usage
#### appSettings.json
```json
{
  "Elasticsearch": {
    "Username": "elastic",
    "Password": "password",
    "Hostname": "localhost"
  }
}
```
#### program.cs
```csharp
using CodeNet.Elasticsearch.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<ElasticsearchModule>();
});
builder.AddElasticsearch("Elasticsearch");
//...

var app = builder.Build();
//...
app.Run();
```