## CodeNet.EntityFramework.InMemory

CodeNet.EntityFramework.InMemory is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.InMemory/) to install CodeNet.EntityFramework.InMemory

```bash
dotnet add package CodeNet.EntityFramework.InMemory
```

### Usage
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddInMemoryDB("DatabaseName");
//...

var app = builder.Build();
//...
app.Run();
```