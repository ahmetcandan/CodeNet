using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Campaign.Abstraction.Repository;

namespace StokTakip.Campaign.Repository
{
    public class CampaignRepository : BaseRepository<Model.Campaign>, ICampaignRepository
    {
        public CampaignRepository(DbContext context) : base(context)
        {

        }
    }
}
