## CodeNet.Redis

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis

```bash
dotnet add package CodeNet.Redis
```

### Usage
appSettings.json
```json
{
  "Redis": {
    "Hostname": "localhost",
    "Port": 6379
  }
}
```
program.cs
```csharp
using CodeNet.Container.Extensions;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.Redis.Module;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"));
//...

var app = builder.Build();
app.UseDistributedCache()
    .UseDistributedLock();
//...
app.Run();
```
#### Usage Lock
```csharp  
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{customerId}")]
    [Lock]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    //...
}
```
#### Usage Cache
```csharp  
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{customerId}")]
    [Cache(10)]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    //...
}
```
