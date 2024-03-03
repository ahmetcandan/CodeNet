using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository
{
    public class ProductRepository : BaseRepository<Model.Product>, IProductRepository
    {
        public ProductRepository(ProductDbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
