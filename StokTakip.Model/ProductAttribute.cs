using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class ProductAttribute : IEntity
    {
        public ProductAttribute()
        {
            ProductAttributeValues = new HashSet<ProductAttributeValue>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategotyId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Categoty { get; set; }
        public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }
    }
}
