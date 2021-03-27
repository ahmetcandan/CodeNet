using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class GiftCardHistory : IEntity
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
