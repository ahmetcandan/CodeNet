using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class ProductPrice : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Tax { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
