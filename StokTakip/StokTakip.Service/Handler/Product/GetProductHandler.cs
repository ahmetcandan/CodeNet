using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Product;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Product
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, ResponseBase<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public GetProductHandler(IProductService ProductService)
        {
            _productService = ProductService;
        }

        public async Task<ResponseBase<ProductViewModel>> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProduct(request.Id, cancellationToken);
            return new ResponseBase<ProductViewModel>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
