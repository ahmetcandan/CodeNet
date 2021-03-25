using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class DiscountCode
    {
        public DiscountCode()
        {
            DiscountCodeRequirements = new HashSet<DiscountCodeRequirement>();
            DiscountCodeUsedHistories = new HashSet<DiscountCodeUsedHistory>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public bool AmountOrRate { get; set; }
        public int? CustomerId { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<DiscountCodeRequirement> DiscountCodeRequirements { get; set; }
        public virtual ICollection<DiscountCodeUsedHistory> DiscountCodeUsedHistories { get; set; }
    }
}
