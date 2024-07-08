using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Api.Controllers;

[Authorize(Roles = "campaign")]
[ApiController]
[Route("[controller]")]
public class CampaignsController(ICampaignService campaignService) : ControllerBase
{
    [HttpGet("{campaignId}")]
    [ProducesResponseType(200, Type = typeof(CampaignResponse))]
    public async Task<IActionResult> Get(int campaignId, CancellationToken cancellationToken)
    {
        return Ok(await campaignService.GetCampaign(campaignId, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CampaignResponse))]
    public async Task<IActionResult> Post(CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        return Ok(await campaignService.CreateCampaign(request, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(CampaignResponse))]
    public async Task<IActionResult> Put(UpdateCampaignRequest request, CancellationToken cancellationToken)
    {
        return Ok(await campaignService.UpdateCampaign(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(CampaignResponse))]
    public async Task<IActionResult> Delete(DeleteCampaignRequest request, CancellationToken cancellationToken)
    {
        return Ok(await campaignService.DeleteCampaign(request.Id, cancellationToken));
    }
}
