using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class SalesOrder
    {
        public SalesOrder()
        {
            CampaignUsedHistories = new HashSet<CampaignUsedHistory>();
            DiscountCodeUsedHistories = new HashSet<DiscountCodeUsedHistory>();
            GiftCardHistories = new HashSet<GiftCardHistory>();
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DiscountCodeId { get; set; }
        public int? GiftCardId { get; set; }
        public int CurrencyTypeId { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DiscountCode DiscountCode { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual GiftCard GiftCard { get; set; }
        public virtual ICollection<CampaignUsedHistory> CampaignUsedHistories { get; set; }
        public virtual ICollection<DiscountCodeUsedHistory> DiscountCodeUsedHistories { get; set; }
        public virtual ICollection<GiftCardHistory> GiftCardHistories { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
