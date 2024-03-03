using NetCore.Abstraction.Model;

namespace StokTakip.Campaign.Model
{
    public partial class GiftCard : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Value { get; set; }
        public int CurrencyTypeId { get; set; }
    }
}
