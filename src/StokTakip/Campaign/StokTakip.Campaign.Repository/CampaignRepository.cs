﻿using CodeNet.EntityFramework;
using StokTakip.Campaign.Abstraction.Repository;

namespace StokTakip.Campaign.Repository;

public class CampaignRepository(CampaignDbContext context) : BaseRepository<Model.Campaign>(context), ICampaignRepository
{
}
