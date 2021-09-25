using Net5Api.Abstraction;
using Net5Api.Abstraction.Enum;
using Net5Api.Cache;
using Net5Api.ExceptionHandling;
using Net5Api.Logging;
using StokTakip.Model;
using System;
using System.Collections.Generic;

namespace StokTakip.Abstraction
{
    public interface ICustomerService : IService
    {
        [Log(LogTime.Before)]
        [Cache(60)]
        public List<CustomerViewModel> GetCustomers();

        [Log(LogTime.Before)]
        [Cache(60)]
        [Exception]
        public CustomerViewModel GetCustomer(int customerId);

        [Log(LogTime.After)]
        public CustomerViewModel CreateCustomer(CustomerViewModel customer);

        [Log(LogTime.After)]
        public CustomerViewModel UpdateCustomer(CustomerViewModel customer);

        [Log(LogTime.After)]
        public CustomerViewModel DeleteCustomer(int customerId);
    }
}
