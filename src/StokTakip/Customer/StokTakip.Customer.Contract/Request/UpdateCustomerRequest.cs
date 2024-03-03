using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Contract.Request
{
    public class UpdateCustomerRequest : IRequest<ResponseBase<CustomerResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
