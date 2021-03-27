using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class Product
    {
        public Product()
        {
            CampaignRequirements = new HashSet<CampaignRequirement>();
            DiscountCodeRequirements = new HashSet<DiscountCodeRequirement>();
            ProductAttributeValues = new HashSet<ProductAttributeValue>();
            ProductPrices = new HashSet<ProductPrice>();
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal TaxRate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<CampaignRequirement> CampaignRequirements { get; set; }
        public virtual ICollection<DiscountCodeRequirement> DiscountCodeRequirements { get; set; }
        public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
