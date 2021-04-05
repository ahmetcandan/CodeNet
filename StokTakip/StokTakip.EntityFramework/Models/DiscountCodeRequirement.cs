#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class DiscountCodeRequirement
    {
        public int Id { get; set; }
        public int DiscountCodeId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }

        public virtual Category Category { get; set; }
        public virtual DiscountCode DiscountCode { get; set; }
        public virtual Product Product { get; set; }
    }
}
