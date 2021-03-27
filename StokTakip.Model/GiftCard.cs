﻿using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class GiftCard : IEntity
    {
        public GiftCard()
        {
            GiftCardHistories = new HashSet<GiftCardHistory>();
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
    }
}
