using NetCore.Abstraction;
using NetCore.ExceptionHandling;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Contract.Request;
using StokTakip.Campaign.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Campaign.Service;

public class CampaignService(ICampaignRepository CampaignRepository) : BaseService, ICampaignService
{
    public async Task<CampaignResponse> CreateCampaign(CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        var result = await CampaignRepository.AddAsync(new Model.Campaign
        {
            Name = request.Name,
            IsActive = true,
            IsDeleted = false
        }, cancellationToken);
        await CampaignRepository.SaveChangesAsync(cancellationToken);
        var response = new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
        return response;
    }

    public async Task<CampaignResponse> DeleteCampaign(int campaignId, CancellationToken cancellationToken)
    {
        var result = await CampaignRepository.GetAsync([campaignId], cancellationToken);
        CampaignRepository.Remove(result);
        await CampaignRepository.SaveChangesAsync(cancellationToken);
        return new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public async Task<CampaignResponse> GetCampaign(int campaignId, CancellationToken cancellationToken)
    {
        var result = await CampaignRepository.GetAsync([campaignId], cancellationToken) ?? throw new UserLevelException("01", "Kampanya bulunamadı!");
        var value = new CampaignResponse
        {
            Name = result.Name,
            Id = result.Id
        };
        return value;
    }

    public async Task<CampaignResponse> UpdateCampaign(UpdateCampaignRequest request, CancellationToken cancellationToken)
    {
        var result = await CampaignRepository.GetAsync([request.Id], cancellationToken);
        result.Name = request.Name;
        CampaignRepository.Update(result);
        await CampaignRepository.SaveChangesAsync(cancellationToken);
        return new CampaignResponse
        {
            Id = result.Id,
            Name = result.Name
        };
    }
}
