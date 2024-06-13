using CodeNet.Abstraction;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Abstraction.Service;

public interface ICustomerService : IService
{
    Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken);

    Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken);

    Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken);

    Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken);
}
