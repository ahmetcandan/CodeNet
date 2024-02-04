using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service
{
    public class CampaignService : BaseService, ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<CampaignViewModel> CreateCampaign(CampaignViewModel campaign, CancellationToken cancellationToken)
        {
            var result = await _campaignRepository.AddAsync(new EntityFramework.Models.Campaign
            {
                Name = campaign.Name,
                IsActive = true,
                IsDeleted = false
            }, cancellationToken);
            await _campaignRepository.SaveChangesAsync(cancellationToken);
            var response = new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
            return response;
        }

        public async Task<CampaignViewModel> DeleteCampaign(int campaignId, CancellationToken cancellationToken)
        {
            var result = await _campaignRepository.GetAsync(campaignId, cancellationToken);
            result.IsDeleted = true;
            _campaignRepository.Update(result);
            await _campaignRepository.SaveChangesAsync(cancellationToken);
            return new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
        }

        public async Task<CampaignViewModel> GetCampaign(int campaignId, CancellationToken cancellationToken)
        {
            var result = await _campaignRepository.GetAsync(campaignId, cancellationToken);
            if (result == null)
                return null;
            var value = new CampaignViewModel
            {
                Name = result.Name,
                Id = result.Id
            };
            return value;
        }

        public async Task<CampaignViewModel> UpdateCampaign(CampaignViewModel campaign, CancellationToken cancellationToken)
        {
            var result = await _campaignRepository.GetAsync(campaign.Id, cancellationToken);
            result.Name = campaign.Name;
            _campaignRepository.Update(result);
            await _campaignRepository.SaveChangesAsync(cancellationToken);
            return new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
}
