using NetCore.Abstraction.Model;

namespace StokTakip.Product.Model
{
    public class ProductAttribute : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategotyId { get; set; }
    }
}
