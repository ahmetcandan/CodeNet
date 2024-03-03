using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, ResponseBase<ProductResponse>>
    {
        private readonly IProductService _productService;

        public DeleteProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<ProductResponse>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.DeleteProduct(request.Id, cancellationToken);
            return new ResponseBase<ProductResponse>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
