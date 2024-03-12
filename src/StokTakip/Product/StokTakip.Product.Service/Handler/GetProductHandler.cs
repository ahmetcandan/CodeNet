using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class GetProductHandler(IProductService ProductService) : IRequestHandler<GetProductRequest, ResponseBase<ProductResponse>>
{
    private readonly IProductService _productService = ProductService;

    public async Task<ResponseBase<ProductResponse>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProduct(request.Id, cancellationToken);
        return new ResponseBase<ProductResponse>
        {
            Data = product,
            IsSuccessfull = true
        };
    }
}
