using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Campaign.Abstraction.Repository;

namespace StokTakip.Campaign.Repository
{
    public class CampaignRepository : BaseRepository<Model.Campaign>, ICampaignRepository
    {
        public CampaignRepository(DbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
