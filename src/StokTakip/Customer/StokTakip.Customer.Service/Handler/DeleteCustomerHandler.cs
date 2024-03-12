using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class DeleteCustomerHandler(ICustomerService customerService) : IRequestHandler<DeleteCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerService.DeleteCustomer(request.Id, cancellationToken);
        return new ResponseBase<CustomerResponse>
        {
            Data = customer,
            IsSuccessfull = true
        };
    }
}
