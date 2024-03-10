using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Contract.Request
{
    public class DeleteCustomerRequest : IRequest<ResponseBase<CustomerResponse>>
    {
        public required int Id { get; set; }
    }
}
