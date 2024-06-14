## CodeNet.EntityFramework

CodeNet.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework/) to install CodeNet.EntityFramework.

```bash
dotnet add package CodeNet.EntityFramework
```

### Usage
#### appSettings.json
```json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=localhost;Initial Catalog=TestDB;TrustServerCertificate=true"
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddSqlServer("SqlServer");
//...

var app = builder.Build();
//...
app.Run();
```