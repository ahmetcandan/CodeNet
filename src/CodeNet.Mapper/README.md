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
builder.AddMapper(c => 
    {
        c.SetMaxDepth(3);

        c.CreateMap<CreateCustomerRequest, Customer>()
            .Map(s => s.Name, d => d.Name)
            .Map(s => s.Number, d => d.No);

        c.CreateMap<CustomerResponse, Customer>()
            .Map(s => s.Name, d => d.Name)
            .Map(s => s.Number, d => d.No)
            .MaxDepth(4);
    });
//...

var app = builder.Build();
//...
app.Run();
```