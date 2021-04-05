using System;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class DiscountCodeUsedHistory
    {
        public int DiscountCodeId { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime UsedDate { get; set; }
        public bool IsCancel { get; set; }

        public virtual DiscountCode DiscountCode { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
