## CodeNet.MongoDB

CodeNet.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MongoDB/) to install CodeNet.MongoDB.

```bash
dotnet add package CodeNet.MongoDB
```

### Usage
#### appSettings.json
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CodeNet"
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMongoDB("MongoDB");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MongoDBModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```