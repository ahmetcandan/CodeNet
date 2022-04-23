using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;

namespace StokTakip.Campaign.Api.Controllers
{
    [Authorize(Roles = "campaign")]
    [ApiController]
    [Route("[controller]")]
    public class CampaignsController : ControllerBase
    {
        ICampaignService CampaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            CampaignService = campaignService;
        }

        [HttpGet]
        public List<CampaignViewModel> GetAll()
        {
            CampaignService.SetUser(User);
            return CampaignService.GetCampaigns();
        }

        [HttpGet("{campaignId}")]
        public CampaignViewModel Get(int campaignId)
        {
            CampaignService.SetUser(User);
            return CampaignService.GetCampaign(campaignId);
        }

        [HttpPost]
        public CampaignViewModel Post(CampaignViewModel campaign)
        {
            CampaignService.SetUser(User);
            return CampaignService.CreateCampaign(campaign);
        }

        [HttpPut]
        public CampaignViewModel Put(CampaignViewModel campaign)
        {
            CampaignService.SetUser(User);
            return CampaignService.UpdateCampaign(campaign);
        }

        [HttpDelete]
        public CampaignViewModel Delete(int campaignId)
        {
            CampaignService.SetUser(User);
            return CampaignService.DeleteCampaign(campaignId);
        }
    }
}
