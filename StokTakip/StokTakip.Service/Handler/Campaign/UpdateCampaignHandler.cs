using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Campaign;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Campaign
{
    public class UpdateCampaignHandler : IRequestHandler<UpdateCampaignRequest, ResponseBase<CampaignViewModel>>
    {
        private readonly ICampaignService _campaignService;

        public UpdateCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignViewModel>> Handle(UpdateCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.UpdateCampaign(new CampaignViewModel 
            {
                Id = request.Id,
                Name = request.Name
            }, cancellationToken);
            return new ResponseBase<CampaignViewModel>
            {
                Data = campaign,
                IsSuccessfull = true
            };
        }
    }
}
