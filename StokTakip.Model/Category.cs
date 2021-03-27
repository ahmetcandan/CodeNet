using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class Category : IEntity
    {
        public Category()
        {
            CampaignRequirements = new HashSet<CampaignRequirement>();
            DiscountCodeRequirements = new HashSet<DiscountCodeRequirement>();
            InverseParent = new HashSet<Category>();
            ProductAttributes = new HashSet<ProductAttribute>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<CampaignRequirement> CampaignRequirements { get; set; }
        public virtual ICollection<DiscountCodeRequirement> DiscountCodeRequirements { get; set; }
        public virtual ICollection<Category> InverseParent { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
