using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Handler;

public class DeleteProductHandler(IProductService ProductService) : IRequestHandler<DeleteProductRequest, ResponseBase<ProductResponse>>
{
    public async Task<ResponseBase<ProductResponse>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        => new ResponseBase<ProductResponse>(await ProductService.DeleteProduct(request.Id, cancellationToken));
}
