using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;

namespace StokTakip.Repository
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(DbContext context, IIdentityContext identityContext) : base(context, identityContext)
        {

        }
    }
}
