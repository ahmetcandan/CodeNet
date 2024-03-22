using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Service.Handler;

public class DeleteCampaignHandler(ICampaignService CampaignService) : IRequestHandler<DeleteCampaignRequest, ResponseBase<CampaignResponse>>
{
    public async Task<ResponseBase<CampaignResponse>> Handle(DeleteCampaignRequest request, CancellationToken cancellationToken)
        => new ResponseBase<CampaignResponse>(await CampaignService.DeleteCampaign(request.Id, cancellationToken));
}
