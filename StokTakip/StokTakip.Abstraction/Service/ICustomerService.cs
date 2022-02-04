using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Cache;
using NetCore.ExceptionHandling;
using NetCore.Logging;
using StokTakip.Model;
using System;
using System.Collections.Generic;

namespace StokTakip.Abstraction
{
    public interface ICustomerService : IService
    {
        //[Log(LogTime.Before)]
        [Cache(120)]
        public List<CustomerViewModel> GetCustomers();

        //[Log(LogTime.Before)]
        [Cache(60)]
        [Exception]
        public CustomerViewModel GetCustomer(int customerId);

        //[Log(LogTime.After)]
        public CustomerViewModel CreateCustomer(CustomerViewModel customer);

        //[Log(LogTime.After)]
        public CustomerViewModel UpdateCustomer(CustomerViewModel customer);

        //[Log(LogTime.After)]
        public CustomerViewModel DeleteCustomer(int customerId);
    }
}
