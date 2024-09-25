## CodeNet.Core

CodeNet.Core is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Core/) to install CodeNet.Core

```bash
dotnet add package CodeNet.Core
```

### Usage
appSettings.json
```json
{
  "Application": {
    "Name": "Customer",
    "Title": "StokTakip | Customer API",
    "Version": "v1.0"
  },
  "JWT": {
    "ValidAudience": "http://codenet",
    "ValidIssuer": "http://login.codenet",
    "PublicKeyPath": "public_key.pem"
  }
}
```
program.cs
```csharp
using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"), options => 
    {
        options.AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"));
    })
//...

var app = builder.Build();
app.UseCodeNet(options =>
{
    options.UseAuthentication();
    options.UseAuthorization();
});
//...
app.Run();
```
