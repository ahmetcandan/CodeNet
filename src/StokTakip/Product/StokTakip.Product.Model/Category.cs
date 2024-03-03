using NetCore.Abstraction.Model;

namespace StokTakip.Product.Model
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
    }
}
