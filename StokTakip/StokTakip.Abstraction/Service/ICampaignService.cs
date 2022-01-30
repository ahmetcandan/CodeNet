using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Cache;
using NetCore.ExceptionHandling;
using NetCore.Logging;
using StokTakip.Model;
using System;
using System.Collections.Generic;

namespace StokTakip.Abstraction
{
    public interface ICampaignService : IService
    {
        [Log(LogTime.Before)]
        [Cache(60)]
        public List<CampaignViewModel> GetCampaigns();

        [Log(LogTime.Before)]
        [Cache(60)]
        [Exception]
        public CampaignViewModel GetCampaign(int campaignId);

        [Log(LogTime.After)]
        public CampaignViewModel CreateCampaign(CampaignViewModel campaign);

        [Log(LogTime.After)]
        public CampaignViewModel UpdateCampaign(CampaignViewModel campaign);

        [Log(LogTime.After)]
        public CampaignViewModel DeleteCampaign(int campaignId);
    }
}
