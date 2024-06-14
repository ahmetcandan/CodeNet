## CodeNet.EntityFramework.InMemory

CodeNet.EntityFramework.InMemory is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.InMemory/) to install CodeNet.EntityFramework.

```bash
dotnet add package CodeNet.EntityFramework.InMemory
```

#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddInMemoryDB("DatabaseName");
//...

var app = builder.Build();
//...
app.Run();
```