using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Abstraction.Service;

public interface IProductService
{
    public Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken);

    public Task<ProductResponse> CreateProduct(CreateProductRequest product, CancellationToken cancellationToken);

    public Task<ProductResponse> UpdateProduct(UpdateProductRequest product, CancellationToken cancellationToken);

    public Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken);
}
