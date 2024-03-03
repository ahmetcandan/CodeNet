using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Campaign.Abstraction.Repository;

namespace StokTakip.Product.Repository
{
    public class CampaignRepository : BaseRepository<Campaign.Model.Campaign>, ICampaignRepository
    {
        public CampaignRepository(CampaignDbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
