## CodeNet.MongoDB

CodeNet.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MongoDB/) to install CodeNet.MongoDB

```bash
dotnet add package CodeNet.MongoDB
```

### Usage
appSettings.json
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CodeNet"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMongoDB(builder.Configuration.GetSection("MongoDB"));
//...

var app = builder.Build();
//...
app.Run();
```
Sample Repositoriy
```csharp
public class SampleRepository(MongoDBContext dbContext) : BaseMongoRepository<KeyValueModel>(dbContext), ISampleRepository
{
    //...
}
```
KeyValueModel
```csharp
[CollectionName("KeyValue")]
public class KeyValueModel : BaseMongoDBModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
```