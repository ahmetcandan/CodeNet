using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Customer;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Customer
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, ResponseBase<CustomerViewModel>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseBase<CustomerViewModel>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomer(request.Id, cancellationToken);
            return new ResponseBase<CustomerViewModel>
            {
                Data = customer,
                IsSuccessfull = true
            };
        }
    }
}
