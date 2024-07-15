## CodeNet.ExceptionHandling

CodeNet.ExceptionHandling is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.ExceptionHandling/) to install CodeNet.ExceptionHandling

```bash
dotnet add package CodeNet.ExceptionHandling
```

### Usage
appSettings.json
```json
{
  "DefaultErrorMessage": {
	"MessageCode": "EX0001",
	"Message": "An unexpected error occurred!"
  }
}
```

program.cs
```csharp
using CodeNet.ExceptionHandling.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDefaultErrorMessage(builder.Configuration.GetSection("DefaultErrorMessage"));
//...

var app = builder.Build();
//...
app.UseExceptionHandling(); //This should be used last.
app.Run();
```

Example Error Message
```json
{
	"Detail": "Default message details",
	"Title": "Default message title",
	"Status": 500
}
```