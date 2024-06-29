## CodeNet.EntityFramework.MySQL

CodeNet.EntityFramework.MySQL is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.MySQL/) to install CodeNet.EntityFramework.MySQL

```bash
dotnet add package CodeNet.EntityFramework.MySQL
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "MySQL": "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMySQL("MySQL");
//...

var app = builder.Build();
//...
app.Run();
```