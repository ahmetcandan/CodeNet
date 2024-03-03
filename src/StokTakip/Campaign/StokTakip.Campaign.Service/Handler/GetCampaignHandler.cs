using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service.Handler
{
    public class GetCampaignHandler : IRequestHandler<GetCampaignRequest, ResponseBase<CampaignResponse>>
    {
        private readonly ICampaignService _campaignService;

        public GetCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignResponse>> Handle(GetCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.GetCampaign(request.Id, cancellationToken);
            return new ResponseBase<CampaignResponse>
            {
                Data = campaign,
                IsSuccessfull = true
            };
        }
    }
}
