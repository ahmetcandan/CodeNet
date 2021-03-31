using Microsoft.EntityFrameworkCore;
using Net5Api.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;
using System;

namespace StokTakip.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context)
        {

        }
    }
}
