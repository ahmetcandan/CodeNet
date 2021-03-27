using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class Customer
    {
        public Customer()
        {
            DiscountCodes = new HashSet<DiscountCode>();
            ProductPrices = new HashSet<ProductPrice>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<DiscountCode> DiscountCodes { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
