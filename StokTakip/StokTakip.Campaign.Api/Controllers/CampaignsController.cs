using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Api.Controllers
{
    [Authorize(Roles = "campaign")]
    [ApiController]
    [Route("[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService ?? throw new System.ArgumentNullException(nameof(campaignService));
        }

        [HttpGet("{campaignId}")]
        public async Task<CampaignViewModel> Get(int campaignId, CancellationToken cancellationToken)
        {
            return await _campaignService.GetCampaign(campaignId, cancellationToken);
        }

        [HttpPost]
        public async Task<CampaignViewModel> Post(CampaignViewModel campaign, CancellationToken cancellationToken)
        {
            return await _campaignService.CreateCampaign(campaign, cancellationToken);
        }

        [HttpPut]
        public async Task<CampaignViewModel> Put(CampaignViewModel campaign, CancellationToken cancellationToken)
        {
            return await _campaignService.UpdateCampaign(campaign, cancellationToken);
        }

        [HttpDelete]
        public async Task<CampaignViewModel> Delete(int campaignId, CancellationToken cancellationToken)
        {
            return await _campaignService.DeleteCampaign(campaignId, cancellationToken);
        }
    }
}
