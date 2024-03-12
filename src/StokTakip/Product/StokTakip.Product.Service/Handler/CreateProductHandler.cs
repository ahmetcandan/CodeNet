using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class CreateProductHandler(IProductService productService) : IRequestHandler<CreateProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.CreateProduct(request, cancellationToken);
        return new ResponseBase<ProductResponse>
        {
            Data = product,
            IsSuccessfull = true
        };
    }
}
