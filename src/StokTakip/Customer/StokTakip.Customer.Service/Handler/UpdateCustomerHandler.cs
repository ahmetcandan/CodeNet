using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class UpdateCustomerHandler(ICustomerService CustomerService) : IRequestHandler<UpdateCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CustomerResponse>(await CustomerService.UpdateCustomer(request, cancellationToken));
}
