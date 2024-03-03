using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Contract.Request
{
    public class GetProductRequest : IRequest<ResponseBase<ProductResponse>>
    {
        public int Id { get; set; }
    }
}
