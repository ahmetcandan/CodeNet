using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using StokTakip.Customer.Abstraction.Repository;

namespace StokTakip.Customer.Repository;

public class CustomerRepository(CustomerDbContext context, IIdentityContext identityContext) : TracingRepository<Model.Customer>(context, identityContext), ICustomerRepository
{
}
