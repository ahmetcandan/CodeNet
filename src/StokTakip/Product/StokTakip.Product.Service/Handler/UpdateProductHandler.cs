using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class UpdateProductHandler(IProductService ProductService) : IRequestHandler<UpdateProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        => new ResponseBase<ProductResponse>(await ProductService.UpdateProduct(request, cancellationToken));
}
