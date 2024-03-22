using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class GetProductHandler(IProductService ProductService) : IRequestHandler<GetProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(GetProductRequest request, CancellationToken cancellationToken) 
        => new ResponseBase<ProductResponse>(await ProductService.GetProduct(request.Id, cancellationToken));
}
