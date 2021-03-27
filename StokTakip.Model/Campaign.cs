using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class Campaign
    {
        public Campaign()
        {
            CampaigUsedHistories = new HashSet<CampaigUsedHistory>();
            CampaignRequirements = new HashSet<CampaignRequirement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public bool AmountOrRate { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }
        public virtual ICollection<CampaigUsedHistory> CampaigUsedHistories { get; set; }
        public virtual ICollection<CampaignRequirement> CampaignRequirements { get; set; }
    }
}
