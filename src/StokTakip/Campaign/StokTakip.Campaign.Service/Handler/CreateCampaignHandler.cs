using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Service.Handler;

public class CreateCampaignHandler(ICampaignService CampaignService) : IRequestHandler<CreateCampaignRequest, ResponseBase<CampaignResponse>>
{
    public async Task<ResponseBase<CampaignResponse>> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CampaignResponse>(await CampaignService.CreateCampaign(request, cancellationToken));
}
