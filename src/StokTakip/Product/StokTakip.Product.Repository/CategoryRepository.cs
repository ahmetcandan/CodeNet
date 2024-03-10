using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository
{
    public class CategoryRepository : TracingRepository<Model.Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
