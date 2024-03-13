using NetCore.Abstraction;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service;

public class CampaignService(ICampaignRepository campaignRepository) : BaseService, ICampaignService
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
        result.IsDeleted = true;
        campaignRepository.Update(result);
        await campaignRepository.SaveChangesAsync(cancellationToken);
        return new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public async Task<CampaignResponse> GetCampaign(int campaignId, CancellationToken cancellationToken)
    {
        var result = await campaignRepository.GetAsync([campaignId], cancellationToken);
        if (result == null)
            return null;
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
