using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Customer
{
    public class GetCustomerById : IRequest<ResponseBase<CustomerViewModel>>
    {
        public int CustomerId { get; set; }
    }
}
