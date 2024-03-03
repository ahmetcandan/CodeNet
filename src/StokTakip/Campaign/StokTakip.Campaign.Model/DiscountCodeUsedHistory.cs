using NetCore.Abstraction.Model;

namespace StokTakip.Campaign.Model
{
    public partial class DiscountCodeUsedHistory : IEntity
    {
        public int DiscountCodeId { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime UsedDate { get; set; }
        public bool IsCancel { get; set; }
    }
}
