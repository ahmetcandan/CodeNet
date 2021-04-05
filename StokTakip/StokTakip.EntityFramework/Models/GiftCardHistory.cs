using System;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class GiftCardHistory
    {
        public int GiftCardId { get; set; }
        public int SalesOrderId { get; set; }
        public decimal UsedAmount { get; set; }
        public DateTime UsedDate { get; set; }
        public bool IsCancel { get; set; }

        public virtual GiftCard GiftCard { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
