using CodeNet.EntityFramework.Models;

namespace StokTakip.Product.Model;

public class ProductAttributeValue : BaseEntity
{
    public int ProductId { get; set; }
    public int ProductAttributeId { get; set; }
    public required string Value { get; set; }
}
