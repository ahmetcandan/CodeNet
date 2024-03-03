using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service.Handler
{
    public class DeleteCampaignHandler : IRequestHandler<DeleteCampaignRequest, ResponseBase<CampaignResponse>>
    {
        private readonly ICampaignService _campaignService;

        public DeleteCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignResponse>> Handle(DeleteCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.DeleteCampaign(request.Id, cancellationToken);
            return new ResponseBase<CampaignResponse>
            {
                Data = campaign,
                IsSuccessfull = true
            };
        }
    }
}
