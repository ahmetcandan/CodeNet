using Microsoft.EntityFrameworkCore;
using StokTakip.Product.Model;

namespace StokTakip.Product.Repository;

public partial class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public virtual DbSet<Model.Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=Product;Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}
