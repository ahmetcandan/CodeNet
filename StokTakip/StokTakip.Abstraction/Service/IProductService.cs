using NetCore.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface IProductService : IService
    {
        public Task<ProductViewModel> GetProduct(int productId, CancellationToken cancellationToken);

        public Task<ProductViewModel> CreateProduct(ProductViewModel product, CancellationToken cancellationToken);

        public Task<ProductViewModel> UpdateProduct(ProductViewModel product, CancellationToken cancellationToken);

        public Task<ProductViewModel> DeleteProduct(int productId, CancellationToken cancellationToken);
    }
}
