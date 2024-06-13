using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class CreateCustomerHandler(ICustomerService CustomerService) : IRequestHandler<CreateCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CustomerResponse>(await CustomerService.CreateCustomer(request, cancellationToken));
}
