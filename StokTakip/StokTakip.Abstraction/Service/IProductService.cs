using NetCore.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;

namespace StokTakip.Abstraction
{
    public interface IProductService : IService
    {
        public List<ProductViewModel> GetProducts();

        public ProductViewModel GetProduct(int productId);

        public ProductViewModel CreateProduct(ProductViewModel product);

        public ProductViewModel UpdateProduct(ProductViewModel product);

        public ProductViewModel DeleteProduct(int productId);
    }
}
