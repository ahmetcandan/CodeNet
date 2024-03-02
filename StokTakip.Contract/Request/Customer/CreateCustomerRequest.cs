using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Customer
{
    public class CreateCustomerRequest : IRequest<ResponseBase<CustomerViewModel>>
    {
        public string Name { get; set; }
        public string No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
