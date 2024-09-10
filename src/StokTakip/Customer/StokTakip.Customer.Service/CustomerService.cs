using CodeNet.ExceptionHandling;
using CodeNet.Mapper.Services;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service;

public class CustomerService(ICustomerRepository customerRepository, ICodeNetMapper mapper) : ICustomerService
{
    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = mapper.MapTo<CreateCustomerRequest, Model.Customer>(request);
        var result = await customerRepository.AddAsync(model, cancellationToken);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapTo<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await customerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("CUS001", $"Customer not found, Id: {customerId}");
        customerRepository.Remove(result);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapTo<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await customerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("CUS001", $"Customer not found, Id: {customerId}");
        return mapper.MapTo<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> UpdateCustomer(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await customerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("CUS001", $"Customer not found, Id: {customerId}");
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.No = request.No;
        customerRepository.Update(result);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapTo<Model.Customer, CustomerResponse>(result);
    }
}
