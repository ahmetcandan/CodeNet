using CodeNet.EntityFramework.Models;

namespace StokTakip.Product.Model;

public class ProductAttribute : BaseEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? CategotyId { get; set; }
}
