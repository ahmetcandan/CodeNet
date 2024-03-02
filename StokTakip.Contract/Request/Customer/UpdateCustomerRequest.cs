using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Customer
{
    public class UpdateCustomerRequest : IRequest<ResponseBase<CustomerViewModel>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
