using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Customer
{
    public class GetCustomerRequest : IRequest<ResponseBase<CustomerViewModel>>
    {
        public int Id { get; set; }
    }
}
