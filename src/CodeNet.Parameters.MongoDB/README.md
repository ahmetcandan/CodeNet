## CodeNet.Parameters.MongoDB

CodeNet.Parameters is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Parameters.MongoDB/) to install CodeNet.Parameters

```bash
dotnet add package CodeNet.Parameters.MongoDB
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
builder.AddParameters(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")), builder.Configuration.GetSection("Identity"));
//...

var app = builder.Build();
//...
app.Run();
```