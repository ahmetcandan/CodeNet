using MediatR;
using CodeNet.Core.Models;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class DeleteCustomerHandler(ICustomerService CustomerService) : IRequestHandler<DeleteCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CustomerResponse>(await CustomerService.DeleteCustomer(request.Id, cancellationToken));
}
