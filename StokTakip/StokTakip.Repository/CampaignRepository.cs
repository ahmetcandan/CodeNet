using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;

namespace StokTakip.Repository
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(DbContext context) : base(context)
        {

        }
    }
}
