using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Campaign
{
    public class CreateCampaignRequest : IRequest<ResponseBase<CampaignViewModel>>
    {
        public string Name { get; set; }
    }
}
