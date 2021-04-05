using System.Collections.Generic;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class CurrencyType
    {
        public CurrencyType()
        {
            Campaigns = new HashSet<Campaign>();
            DiscountCodes = new HashSet<DiscountCode>();
            GiftCards = new HashSet<GiftCard>();
            ProductPrices = new HashSet<ProductPrice>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SortName { get; set; }
        public string Symbol { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<DiscountCode> DiscountCodes { get; set; }
        public virtual ICollection<GiftCard> GiftCards { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
