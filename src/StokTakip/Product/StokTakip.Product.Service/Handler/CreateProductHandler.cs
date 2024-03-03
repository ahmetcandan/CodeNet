using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, ResponseBase<ProductResponse>>
    {
        private readonly IProductService _productService;

        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<ProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.CreateProduct(request, cancellationToken);
            return new ResponseBase<ProductResponse>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
