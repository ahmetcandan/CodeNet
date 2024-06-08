using NetCore.Abstraction;
using NetCore.EntityFramework;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository;

public class CategoryRepository(ProductDbContext context, IIdentityContext identityContext) : TracingRepository<Model.Category>(context, identityContext), ICategoryRepository
{
}
