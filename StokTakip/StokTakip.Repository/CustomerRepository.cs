using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;

namespace StokTakip.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context)
        {

        }
    }
}
