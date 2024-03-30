using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class GetCustomerHandler(ICustomerService CustomerService) : IRequestHandler<GetCustomerRequest, ResponseBase<CustomerResponse>>
{
    [Cache(Time = 10)]
    public async Task<ResponseBase<CustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CustomerResponse>(await CustomerService.GetCustomer(request.Id, cancellationToken));
}
