using CodeNet.Abstraction;
using CodeNet.EntityFramework;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository;

public class CategoryRepository(ProductDbContext context, IIdentityContext identityContext) : TracingRepository<Model.Category>(context, identityContext), ICategoryRepository
{
}
