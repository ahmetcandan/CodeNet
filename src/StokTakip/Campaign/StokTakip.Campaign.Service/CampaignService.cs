using CodeNet.ExceptionHandling;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Service;

public class CampaignService(ICampaignRepository campaignRepository) : ICampaignService
{
    public async Task<CampaignResponse> CreateCampaign(CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        var result = await campaignRepository.AddAsync(new Model.Campaign
        {
            Name = request.Name,
            IsActive = true,
            IsDeleted = false
        }, cancellationToken);
        await campaignRepository.SaveChangesAsync(cancellationToken);
        var response = new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
        return response;
    }

    public async Task<CampaignResponse> DeleteCampaign(int campaignId, CancellationToken cancellationToken)
    {
        var result = await campaignRepository.GetAsync([campaignId], cancellationToken);
        campaignRepository.Remove(result);
        await campaignRepository.SaveChangesAsync(cancellationToken);
        return new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public async Task<CampaignResponse> GetCampaign(int campaignId, CancellationToken cancellationToken)
    {
        var result = await campaignRepository.GetAsync([campaignId], cancellationToken) ?? throw new UserLevelException("01", "Kampanya bulunamadı!");
        var value = new CampaignResponse
        {
            Name = result.Name,
            Id = result.Id
        };
        return value;
    }

    public async Task<CampaignResponse> UpdateCampaign(UpdateCampaignRequest request, CancellationToken cancellationToken)
    {
        var result = await campaignRepository.GetAsync([request.Id], cancellationToken);
        result.Name = request.Name;
        campaignRepository.Update(result);
        await campaignRepository.SaveChangesAsync(cancellationToken);
        return new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
    }
}
