using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Customer.Abstraction.Repository;

namespace StokTakip.Customer.Repository
{
    public class CustomerRepository : TracingRepository<Model.Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {
        }
    }
}
