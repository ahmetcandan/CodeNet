## CodeNet.Mapper

CodeNet.Mapper is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Mapper/) to install CodeNet.Mapper

```bash
dotnet add package CodeNet.Mapper
```

### Usage
program.cs
```csharp
using CodeNet.Mapper.Module;

var builder = WebApplication.CreateBuilder(args);
builder.AddMapper();
//...

var app = builder.Build();
//...
app.Run();
```