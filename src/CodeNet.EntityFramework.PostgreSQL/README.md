## CodeNet.EntityFramework.PostgreSQL

CodeNet.EntityFramework.PostgreSQL is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.PostgreSQL/) to install CodeNet.EntityFramework.PostgreSQL

```bash
dotnet add package CodeNet.EntityFramework.PostgreSQL
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddNpgsql("PostgreSQL");
//...

var app = builder.Build();
//...
app.Run();
```