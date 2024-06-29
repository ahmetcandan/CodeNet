## CodeNet.EntityFramework

CodeNet.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework/) to install CodeNet.EntityFramework

```bash
dotnet add package CodeNet.EntityFramework
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
> program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
});
builder.AddSqlServer<CustomerDbContext>("SqlServer");
//...

var app = builder.Build();
//...
app.Run();
```
> DbContext
```csharp
public partial class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public virtual DbSet<Model.Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
}
```
> Repository
```csharp
public class CustomerRepository(CustomerDbContext context, IIdentityContext identityContext) : 
    TracingRepository<Model.Customer>(context, identityContext), ICustomerRepository
{
}
```

> Repository Usage
```csharp
public class CustomerService(ICustomerRepository CustomerRepository, IAutoMapperConfiguration Mapper) : BaseService, ICustomerService
{
    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = Mapper.MapObject<CreateCustomerRequest, Model.Customer>(request);
        var result = await CustomerRepository.AddAsync(model, cancellationToken);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken);
        CustomerRepository.Remove(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("01", "Kullanıcı bulunamadı!");
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([request.Id], cancellationToken);
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.No = request.No;
        CustomerRepository.Update(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }
}
```