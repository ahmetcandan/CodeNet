## CodeNet.HttpClient

CodeNet.HttpClient is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HttpClient/) to install CodeNet.HttpClient

```bash
dotnet add package CodeNet.HttpClient
```

### Usage
program.cs
```csharp
using CodeNet.HttpClient.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddHttpClient();
//...

var app = builder.Build();
//...
app.Run();
```