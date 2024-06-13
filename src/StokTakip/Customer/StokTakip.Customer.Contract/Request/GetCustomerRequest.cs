using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Contract.Request;

public class GetCustomerRequest : IRequest<ResponseBase<CustomerResponse>>
{
    public required int Id { get; set; }
}
