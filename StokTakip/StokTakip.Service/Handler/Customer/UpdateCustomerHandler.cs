using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Customer;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Customer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, ResponseBase<CustomerViewModel>>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseBase<CustomerViewModel>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.UpdateCustomer(new CustomerViewModel 
            {
                Id = request.Id,
                Code = request.Code,
                Description = request.Description,
                Name = request.Name,
                No = request.No
            }, cancellationToken);
            return new ResponseBase<CustomerViewModel>
            {
                Data = customer,
                IsSuccessfull = true
            };
        }
    }
}
