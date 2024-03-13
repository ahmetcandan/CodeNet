using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Campaign.Abstraction.Repository;

namespace StokTakip.Campaign.Repository;

public class CampaignRepository(DbContext context) : BaseRepository<Model.Campaign>(context), ICampaignRepository
{
}
