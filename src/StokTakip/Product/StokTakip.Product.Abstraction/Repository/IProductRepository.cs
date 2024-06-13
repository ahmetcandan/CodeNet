using CodeNet.Abstraction;
using StokTakip.Product.Model.ViewModel;

namespace StokTakip.Product.Abstraction.Repository;

public interface IProductRepository : ITracingRepository<Model.Product>
{
    Task<ProductInfo?> GetProduct(int productId, CancellationToken cancellationToken);
}
