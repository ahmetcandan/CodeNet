using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service.Handler;

public class GetCampaignHandler(ICampaignService campaignService) : IRequestHandler<GetCampaignRequest, ResponseBase<CampaignResponse>>
{
    public async Task<ResponseBase<CampaignResponse>> Handle(GetCampaignRequest request, CancellationToken cancellationToken)
    {
        var campaign = await campaignService.GetCampaign(request.Id, cancellationToken);
        return new ResponseBase<CampaignResponse>
        {
            Data = campaign,
            IsSuccessfull = true
        };
    }
}
