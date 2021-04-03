using Net5Api.Abstraction;
using StokTakip.EntityFramework.Models;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
