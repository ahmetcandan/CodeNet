using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using StokTakip.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace StokTakip.Service
{
    public class CampaignService : BaseService, ICampaignService
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly ILogRepository logRepository;
        private readonly IQService qService;

        public CampaignService(DbContext dbContext, ILogRepository logRepository, IQService qService)
        {
            campaignRepository = new CampaignRepository(dbContext);
            this.qService = qService;
            this.logRepository = logRepository;
        }

        public override void SetUser(IPrincipal user)
        {
            campaignRepository.SetUser(user);
            base.SetUser(user);
        }

        public CampaignViewModel CreateCampaign(CampaignViewModel campaign)
        {
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
