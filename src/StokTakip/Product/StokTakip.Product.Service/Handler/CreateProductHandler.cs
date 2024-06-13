using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class CreateProductHandler(IProductService ProductService) : IRequestHandler<CreateProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        => new ResponseBase<ProductResponse>(await ProductService.CreateProduct(request, cancellationToken));
}
