using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Product;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Product
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, ResponseBase<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public DeleteProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<ProductViewModel>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.DeleteProduct(request.Id, cancellationToken);
            return new ResponseBase<ProductViewModel>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
