using Net5Api.Abstraction;
using Net5Api.Cache;
using StokTakip.EntityFramework.Models;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface ICustomerService : IService
    {
        [Cache(10830)]
        public List<CustomerViewModel> GetCustomers();

        [Cache(10830)]
        public CustomerViewModel GetCustomer(int customerId);

        public CustomerViewModel CreateCustomer(CustomerViewModel customer);

        public CustomerViewModel UpdateCustomer(CustomerViewModel customer);

        public CustomerViewModel DeleteCustomer(int customerId);
    }
}
