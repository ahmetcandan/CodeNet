using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Campaign;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Campaign
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignRequest, ResponseBase<CampaignViewModel>>
    {
        private readonly ICampaignService _campaignService;

        public CreateCampaignHandler(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<ResponseBase<CampaignViewModel>> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignService.CreateCampaign(new CampaignViewModel 
            {
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
