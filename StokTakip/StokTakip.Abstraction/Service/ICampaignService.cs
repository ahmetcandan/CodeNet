using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Cache;
using NetCore.ExceptionHandling;
using NetCore.Logging;
using StokTakip.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface ICampaignService : IService
    {
        [Log(LogTime.Before)]
        [Cache(60)]
        [Exception]
        public Task<CampaignViewModel> GetCampaign(int campaignId, CancellationToken cancellationToken);

        [Log(LogTime.After)]
        public Task<CampaignViewModel> CreateCampaign(CampaignViewModel campaign, CancellationToken cancellationToken);

        [Log(LogTime.After)]
        public Task<CampaignViewModel> UpdateCampaign(CampaignViewModel campaign, CancellationToken cancellationToken);

        [Log(LogTime.After)]
        public Task<CampaignViewModel> DeleteCampaign(int campaignId, CancellationToken cancellationToken);
    }
}
