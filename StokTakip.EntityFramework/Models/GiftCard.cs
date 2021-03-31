using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class GiftCard
    {
        public GiftCard()
        {
            GiftCardHistories = new HashSet<GiftCardHistory>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Value { get; set; }
        public int CurrencyTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual ICollection<GiftCardHistory> GiftCardHistories { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
