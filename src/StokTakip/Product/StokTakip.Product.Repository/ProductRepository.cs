using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository
{
    public class ProductRepository : BaseRepository<Model.Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {

        }
    }
}
