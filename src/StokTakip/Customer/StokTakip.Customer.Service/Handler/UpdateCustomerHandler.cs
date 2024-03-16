using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class UpdateCustomerHandler(ICustomerService customerService) : IRequestHandler<UpdateCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        return new ResponseBase<CustomerResponse>
        {
            Data = await customerService.UpdateCustomer(request, cancellationToken),
            IsSuccessfull = true
        };
    }
}
