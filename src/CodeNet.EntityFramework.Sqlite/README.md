## CodeNet.EntityFramework.Sqlite

CodeNet.EntityFramework.Sqlite is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Sqlite/) to install CodeNet.EntityFramework.Sqlite.

```bash
dotnet add package CodeNet.EntityFramework.Sqlite
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "Sqlite": "Data Source=c:\mydb.db;Version=3;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddSqlite("Sqlite");
//...

var app = builder.Build();
//...
app.Run();
```