using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;
using System.Linq;

namespace StokTakip.Service
{
    public class CampaignService : BaseService, ICampaignService
    {
        ICampaignRepository campaignRepository;
        ILogRepository logRepository;
        IQService qService;

        public CampaignService(ICampaignRepository campaignRepository, ILogRepository logRepository, IQService qService)
        {
            this.campaignRepository = campaignRepository;
            this.qService = qService;
            this.logRepository = logRepository;
        }

        public CampaignViewModel CreateCampaign(CampaignViewModel campaign)
        {
            campaignRepository.SetUser(GetUser());
            var result = campaignRepository.Add(new EntityFramework.Models.Campaign
            {
                Name = campaign.Name,
                IsActive = true,
                IsDeleted = false
            });
            campaignRepository.SaveChanges();
            var response = new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
            return response;
        }

        public CampaignViewModel DeleteCampaign(int campaignId)
        {
            campaignRepository.SetUser(GetUser());
            var result = campaignRepository.Get(campaignId);
            result.IsDeleted = true;
            campaignRepository.Update(result);
            campaignRepository.SaveChanges();
            return new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
        }

        public CampaignViewModel GetCampaign(int campaignId)
        {
            campaignRepository.SetUser(GetUser());
            var result = campaignRepository.Get(campaignId);
            if (result == null)
                return null;
            var value = new CampaignViewModel
            {
                Name = result.Name,
                Id = result.Id
            };
            return value;
        }

        public List<CampaignViewModel> GetCampaigns()
        {
            campaignRepository.SetUser(GetUser());
            var result = (
                    from c in campaignRepository.GetAll()
                    select new CampaignViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }
                ).ToList();
            return result;
        }

        public CampaignViewModel UpdateCampaign(CampaignViewModel campaign)
        {
            campaignRepository.SetUser(GetUser());
            var result = campaignRepository.Get(campaign.Id);
            result.Name = campaign.Name;
            campaignRepository.Update(result);
            campaignRepository.SaveChanges();
            return new CampaignViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
}
