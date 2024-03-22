using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Service.Handler;

public class UpdateCampaignHandler(ICampaignService CampaignService) : IRequestHandler<UpdateCampaignRequest, ResponseBase<CampaignResponse>>
{
    public async Task<ResponseBase<CampaignResponse>> Handle(UpdateCampaignRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CampaignResponse>(await CampaignService.UpdateCampaign(request, cancellationToken));
}
