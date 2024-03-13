using Microsoft.EntityFrameworkCore;
using StokTakip.Campaign.Model;

namespace StokTakip.Campaign.Repository;

public partial class CampaignDbContext(DbContextOptions<CampaignDbContext> options) : DbContext(options)
{
    public virtual DbSet<Campaign.Model.Campaign> Campaigns { get; set; }
    public virtual DbSet<CampaignRequirement> CampaignRequirements { get; set; }
    public virtual DbSet<CampaignUsedHistory> CampaignUsedHistories { get; set; }
    public virtual DbSet<DiscountCode> DiscountCodes { get; set; }
    public virtual DbSet<DiscountCodeRequirement> DiscountCodeRequirements { get; set; }
    public virtual DbSet<DiscountCodeUsedHistory> DiscountCodeUsedHistories { get; set; }
    public virtual DbSet<GiftCard> GiftCards { get; set; }
    public virtual DbSet<GiftCardHistory> GiftCardHistories { get; set; }
}
