## CodeNet.EntityFramework.Oracle

CodeNet.EntityFramework.Oracle is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Oracle/) to install CodeNet.EntityFramework.Oracle

```bash
dotnet add package CodeNet.EntityFramework.Oracle
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=MyOracleDB;Integrated Security=yes;"
  }
}
```
program.cs
```csharp
using CodeNet.EntityFramework.Oracle.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddOracle(builder.Configuration.GetConnectionString("Oracle"));
//...

var app = builder.Build();
//...
app.Run();
```