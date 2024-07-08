## CodeNet.Parameters

CodeNet.Parameters is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Parameters/) to install CodeNet.Parameters

```bash
dotnet add package CodeNet.Parameters
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=localhost;Initial Catalog=TestDB;TrustServerCertificate=true"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddParameters(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");
//...

var app = builder.Build();
//...
app.Run();
```