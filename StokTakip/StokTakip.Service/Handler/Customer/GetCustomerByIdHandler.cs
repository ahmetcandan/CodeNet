using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Customer;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Customer
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, ResponseBase<CustomerViewModel>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerByIdHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseBase<CustomerViewModel>> Handle(GetCustomerById request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomer(request.CustomerId, cancellationToken);
            return new ResponseBase<CustomerViewModel>
            {
                Data = customer,
                IsSuccessfull = true
            };
        }
    }
}
