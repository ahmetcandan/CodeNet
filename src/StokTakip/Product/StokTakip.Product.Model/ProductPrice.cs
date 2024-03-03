using NetCore.Abstraction.Model;

namespace StokTakip.Product.Model
{
    public class ProductPrice : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Tax { get; set; }
    }
}
