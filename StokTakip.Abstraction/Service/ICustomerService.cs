using Net5Api.Abstraction;
using Net5Api.Cache;
using Net5Api.Logging;
using StokTakip.EntityFramework.Models;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface ICustomerService : IService
    {
        [Log(LogType.Before)]
        [Cache(11030)]
        public List<CustomerViewModel> GetCustomers();

        [Log(LogType.Before)]
        [Cache(11030)]
        public CustomerViewModel GetCustomer(int customerId);

        public CustomerViewModel CreateCustomer(CustomerViewModel customer);

        public CustomerViewModel UpdateCustomer(CustomerViewModel customer);

        public CustomerViewModel DeleteCustomer(int customerId);
    }
}
