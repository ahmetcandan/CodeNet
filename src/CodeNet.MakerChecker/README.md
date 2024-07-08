## CodeNet.MakerChecker

CodeNet.MakerChecker is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MakerChecker/) to install CodeNet.MakerChecker

```bash
dotnet add package CodeNet.MakerChecker
```

### Usage
> appSettings.json
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
builder.AddMakerChecker(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");
//...

var app = builder.Build();
//...
app.Run();
```