using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Abstraction.Service;

public interface ICampaignService
{
    public Task<CampaignResponse> GetCampaign(int campaignId, CancellationToken cancellationToken);

    public Task<CampaignResponse> CreateCampaign(CreateCampaignRequest request, CancellationToken cancellationToken);

    public Task<CampaignResponse> UpdateCampaign(UpdateCampaignRequest request, CancellationToken cancellationToken);

    public Task<CampaignResponse> DeleteCampaign(int campaignId, CancellationToken cancellationToken);
}
