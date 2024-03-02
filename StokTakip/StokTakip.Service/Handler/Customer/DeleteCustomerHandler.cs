using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Customer;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Customer
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, ResponseBase<CustomerViewModel>>
    {
        private readonly ICustomerService _customerService;

        public DeleteCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseBase<CustomerViewModel>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.DeleteCustomer(request.Id, cancellationToken);
            return new ResponseBase<CustomerViewModel>
            {
                Data = customer,
                IsSuccessfull = true
            };
        }
    }
}
