using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class GetCustomerHandler(ICustomerService customerService) : IRequestHandler<GetCustomerRequest, ResponseBase<CustomerResponse>>
{
    [Cache(10)]
    public async Task<ResponseBase<CustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        return new ResponseBase<CustomerResponse>
        {
            Data = await customerService.GetCustomer(request.Id, cancellationToken),
            IsSuccessfull = true
        };
    }
}
