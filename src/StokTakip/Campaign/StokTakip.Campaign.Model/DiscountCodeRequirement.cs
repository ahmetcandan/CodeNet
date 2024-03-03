using NetCore.Abstraction.Model;

namespace StokTakip.Campaign.Model
{
    public partial class DiscountCodeRequirement : IEntity
    {
        public int Id { get; set; }
        public int DiscountCodeId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }
    }
}
