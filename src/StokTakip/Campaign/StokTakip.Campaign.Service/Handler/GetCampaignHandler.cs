using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Service.Handler;

public class GetCampaignHandler(ICampaignService CampaignService) : IRequestHandler<GetCampaignRequest, ResponseBase<CampaignResponse>>
{
    public async Task<ResponseBase<CampaignResponse>> Handle(GetCampaignRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CampaignResponse>(await CampaignService.GetCampaign(request.Id, cancellationToken));
}
