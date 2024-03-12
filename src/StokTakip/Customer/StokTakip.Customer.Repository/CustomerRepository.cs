using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Customer.Abstraction.Repository;

namespace StokTakip.Customer.Repository;

public class CustomerRepository(DbContext context, IIdentityContext identityContext) : TracingRepository<Model.Customer>(context, identityContext), ICustomerRepository
{
}
