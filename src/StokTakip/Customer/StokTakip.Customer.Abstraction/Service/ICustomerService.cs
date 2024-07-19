using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Abstraction.Service;

public interface ICustomerService
{
    Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken);

    Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken);

    Task<CustomerResponse> UpdateCustomer(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken);

    Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken);
}
