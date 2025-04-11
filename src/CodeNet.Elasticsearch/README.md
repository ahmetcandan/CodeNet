## CodeNet.Elasticsearch

CodeNet.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Elasticsearch/) to install CodeNet.Elasticsearch

```bash
dotnet add package CodeNet.Elasticsearch
```

### Usage
appSettings.json
```json
{
  "Elasticsearch": {
    "Hostname": "localhost:9200",
    "Username": "elastic",
    "Password": "password"
  }
}
```

program.cs
```csharp
using CodeNet.Elasticsearch.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddElasticsearch(Configuration.GetSection("Elasticsearch"));
//...

var app = builder.Build();
//...
app.Run();
```

Repository
```csharp
public class TestElasticRepository(ElasticsearchDbContext dbContext) : ElasticsearchRepository<ElasticModel>(dbContext)
{
}
```

Model
```csharp
[IndexName("Test")]
public class ElasticModel : IElasticsearchModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
```