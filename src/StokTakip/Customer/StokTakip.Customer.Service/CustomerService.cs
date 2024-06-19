using CodeNet.ExceptionHandling;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using StokTakip.Customer.Service.Mapper;

namespace StokTakip.Customer.Service;

public class CustomerService(ICustomerRepository CustomerRepository, IAutoMapperConfiguration Mapper) : ICustomerService
{
    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = Mapper.MapObject<CreateCustomerRequest, Model.Customer>(request);
        var result = await CustomerRepository.AddAsync(model, cancellationToken);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken);
        CustomerRepository.Remove(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("01", "Kullanıcı bulunamadı!");
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([request.Id], cancellationToken);
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.No = request.No;
        CustomerRepository.Update(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }
}
