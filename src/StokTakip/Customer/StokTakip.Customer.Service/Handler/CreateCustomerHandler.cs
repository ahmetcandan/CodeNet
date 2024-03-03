using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, ResponseBase<CustomerResponse>>
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseBase<CustomerResponse>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.CreateCustomer(request, cancellationToken);
            return new ResponseBase<CustomerResponse>
            {
                Data = customer,
                IsSuccessfull = true
            };
        }
    }
}
