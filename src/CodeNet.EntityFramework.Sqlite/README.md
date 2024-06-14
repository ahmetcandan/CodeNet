## CodeNet.EntityFramework.PostgreSQL

CodeNet.EntityFramework.PostgreSQL is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.PostgreSQL/) to install CodeNet.EntityFramework.PostgreSQL.

```bash
dotnet add package CodeNet.EntityFramework.PostgreSQL
```

### Usage
#### appSettings.json
```json
{
  "ConnectionStrings": {
    "Sqlite": "Data Source=c:\mydb.db;Version=3;"
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddSqlite("Sqlite");
//...

var app = builder.Build();
//...
app.Run();
```