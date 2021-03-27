using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class SalesOrder : IEntity
    {
        public SalesOrder()
        {
            CampaigUsedHistories = new HashSet<CampaigUsedHistory>();
            DiscountCodeUsedHistories = new HashSet<DiscountCodeUsedHistory>();
            GiftCardHistories = new HashSet<GiftCardHistory>();
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int CurrencyTypeId { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<CampaigUsedHistory> CampaigUsedHistories { get; set; }
        public virtual ICollection<DiscountCodeUsedHistory> DiscountCodeUsedHistories { get; set; }
        public virtual ICollection<GiftCardHistory> GiftCardHistories { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
