using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Contract.Request;

public class GetProductRequest : IRequest<ResponseBase<ProductResponse>>
{
    public required int Id { get; set; }
}
