using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service.Handler
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignRequest, ResponseBase<CampaignResponse>>
    {
        private readonly ICampaignService _campaignService;

        public CreateCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignResponse>> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.CreateCampaign(request, cancellationToken);
            return new ResponseBase<CampaignResponse>
            {
                Data = campaign,
                IsSuccessfull = true
            };
        }
    }
}
