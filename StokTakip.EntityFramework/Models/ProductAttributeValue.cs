using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class ProductAttributeValue
    {
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string Value { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductAttribute ProductAttribute { get; set; }
    }
}
