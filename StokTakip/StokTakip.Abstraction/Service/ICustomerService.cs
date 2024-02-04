using NetCore.Abstraction;
using NetCore.Cache;
using NetCore.ExceptionHandling;
using StokTakip.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface ICustomerService : IService
    {
        //[Log(LogTime.Before)]
        [Cache(60)]
        [Exception]
        Task<CustomerViewModel> GetCustomer(int customerId, CancellationToken cancellationToken);

        //[Log(LogTime.After)]
        Task<CustomerViewModel> CreateCustomer(CustomerViewModel customer, CancellationToken cancellationToken);

        //[Log(LogTime.After)]
        Task<CustomerViewModel> UpdateCustomer(CustomerViewModel customer, CancellationToken cancellationToken);

        //[Log(LogTime.After)]
        Task<CustomerViewModel> DeleteCustomer(int customerId, CancellationToken cancellationToken);
    }
}
