using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Model;
using StokTakip.Product.Model.ViewModel;

namespace StokTakip.Product.Repository;

public class ProductRepository : TracingRepository<Model.Product>, IProductRepository
{
    private readonly DbSet<Category> _categories;

    public ProductRepository(DbContext context, IIdentityContext identityContext) : base(context, identityContext)
    {
        _categories = _dbContext.Set<Category>();
    }

    public async Task<ProductInfo?> GetProduct(int productId, CancellationToken cancellationToken)
    {
        return await (from p in _entities
                      join c in _categories on p.CategoryId equals c.Id
                      where p.Id.Equals(productId)
                      select new ProductInfo
                      {
                          Id = p.Id,
                          Name = p.Name,
                          Code = p.Code,
                          Description = p.Description,
                          Barcode = p.Barcode,
                          TaxRate = p.TaxRate,
                          CategoryId = p.CategoryId,
                          CategoryCode = c.Code,
                          CategoryName = c.Name
                      }).FirstOrDefaultAsync(cancellationToken);
    }
}
