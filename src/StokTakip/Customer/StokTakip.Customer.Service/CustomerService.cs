using CodeNet.ExceptionHandling;
using CodeNet.Parameters.Manager;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using StokTakip.Customer.Service.Mapper;

namespace StokTakip.Customer.Service;

public class CustomerService(ICustomerRepository customerRepository, IAutoMapperConfiguration mapper, IParameterManager parameterManager) : ICustomerService
{
    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var x = await parameterManager.AddParameterGroupWithParamsAsync(new CodeNet.Parameters.Models.AddParameterGroupWithParamsModel
        {
            AddParameters = new List<CodeNet.Parameters.Models.AddParameterModel>
            {
                new CodeNet.Parameters.Models.AddParameterModel
                {
                    Code = "E",
                    Order = 1,
                    Value = "Erkek"
                },
                new CodeNet.Parameters.Models.AddParameterModel
                {
                    Code = "K",
                    Order = 2,
                    Value = "Kadın"
                }
            },
            Code = "CINSIYET",
            ApprovalRequired = false,
            Description = "Cinsiyet"
        }, cancellationToken);
        var model = mapper.MapObject<CreateCustomerRequest, Model.Customer>(request);
        var result = await customerRepository.AddAsync(model, cancellationToken);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await customerRepository.GetAsync([customerId], cancellationToken);
        customerRepository.Remove(result);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
    {
        var prms = await parameterManager.GetParametersAsync("CINSIYET", cancellationToken);
        var p1 = await parameterManager.GetParameterAsync(prms.Data[0].Id, cancellationToken);
        var p2 = await parameterManager.GetParameterAsync(prms.Data[1].Id, cancellationToken);
        await parameterManager.GetParameterGroupAsync("CINSIYET", cancellationToken);
        var result = await customerRepository.GetAsync([customerId], cancellationToken) ?? throw new CodeNetException("01", "Kullanıcı bulunamadı!");
        return mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await customerRepository.GetAsync([request.Id], cancellationToken);
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.No = request.No;
        customerRepository.Update(result);
        await customerRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }
}
