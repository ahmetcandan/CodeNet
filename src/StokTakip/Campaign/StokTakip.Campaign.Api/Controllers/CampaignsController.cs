using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Api.Controllers;

[Authorize(Roles = "campaign")]
[ApiController]
[Route("[controller]")]
public class CampaignsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{campaignId}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CampaignResponse>))]
    public async Task<IActionResult> Get(int CampaignId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCampaignRequest { Id = CampaignId }, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CampaignResponse>))]
    public async Task<IActionResult> Post(CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CampaignResponse>))]
    public async Task<IActionResult> Put(UpdateCampaignRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CampaignResponse>))]
    public async Task<IActionResult> Delete(int CampaignId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new DeleteCampaignRequest { Id = CampaignId }, cancellationToken));
    }
}
