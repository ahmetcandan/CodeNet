using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Contract.Request
{
    public class DeleteProductRequest : IRequest<ResponseBase<ProductResponse>>
    {
        public required int Id { get; set; }
    }
}
