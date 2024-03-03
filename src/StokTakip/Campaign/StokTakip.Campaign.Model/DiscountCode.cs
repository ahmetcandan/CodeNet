using NetCore.Abstraction.Model;

namespace StokTakip.Campaign.Model
{
    public partial class DiscountCode : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public bool AmountOrRate { get; set; }
        public int? CustomerId { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
