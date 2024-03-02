using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Contract.Request.Campaign;
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
        private readonly IMediator _mediator;

        public CampaignsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{campaignId}")]
        public async Task<ResponseBase<CampaignViewModel>> Get(int CampaignId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetCampaignRequest { Id = CampaignId }, cancellationToken);
        }

        [HttpPost]
        public async Task<ResponseBase<CampaignViewModel>> Post(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpPut]
        public async Task<ResponseBase<CampaignViewModel>> Put(UpdateCampaignRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpDelete]
        public async Task<ResponseBase<CampaignViewModel>> Delete(int CampaignId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new DeleteCampaignRequest { Id = CampaignId }, cancellationToken);
        }
    }
}
