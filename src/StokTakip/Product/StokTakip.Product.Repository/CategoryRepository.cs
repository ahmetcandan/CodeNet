using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using StokTakip.Product.Abstraction.Repository;

namespace StokTakip.Product.Repository;

public class CategoryRepository(ProductDbContext context, ICodeNetContext codeNetContext) : TracingRepository<Model.Category>(context, codeNetContext), ICategoryRepository
{
}
