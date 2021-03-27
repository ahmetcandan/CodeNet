using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class CampaignRequirement
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Category Category { get; set; }
        public virtual Product Product { get; set; }
    }
}
