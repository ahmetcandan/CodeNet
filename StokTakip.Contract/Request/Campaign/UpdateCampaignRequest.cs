using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Campaign
{
    public class UpdateCampaignRequest : IRequest<ResponseBase<CampaignViewModel>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
