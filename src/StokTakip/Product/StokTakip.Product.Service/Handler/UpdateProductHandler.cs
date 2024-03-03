using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, ResponseBase<ProductResponse>>
    {
        private readonly IProductService _productService;

        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<ProductResponse>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.UpdateProduct(request, cancellationToken);
            return new ResponseBase<ProductResponse>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
