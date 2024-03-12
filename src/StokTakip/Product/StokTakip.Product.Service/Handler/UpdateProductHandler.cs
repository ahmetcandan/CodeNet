using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class UpdateProductHandler(IProductService productService) : IRequestHandler<UpdateProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.UpdateProduct(request, cancellationToken);
        return new ResponseBase<ProductResponse>
        {
            Data = product,
            IsSuccessfull = true
        };
    }
}
