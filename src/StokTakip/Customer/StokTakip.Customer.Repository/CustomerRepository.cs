using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Customer.Abstraction.Repository;

namespace StokTakip.Customer.Repository
{
    public class CustomerRepository : BaseRepository<Model.Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerDbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
