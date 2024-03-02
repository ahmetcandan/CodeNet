using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Campaign;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Campaign
{
    public class DeleteCampaignHandler : IRequestHandler<DeleteCampaignRequest, ResponseBase<CampaignViewModel>>
    {
        private readonly ICampaignService _campaignService;

        public DeleteCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignViewModel>> Handle(DeleteCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.DeleteCampaign(request.Id, cancellationToken);
            return new ResponseBase<CampaignViewModel>
            {
                Data = campaign,
                IsSuccessfull = true
            };
        }
    }
}
