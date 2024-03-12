using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository;

public class CategoryRepository(DbContext context, IIdentityContext identityContext) : TracingRepository<Model.Category>(context, identityContext), ICategoryRepository
{
}
