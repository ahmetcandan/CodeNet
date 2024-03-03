using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Contract.Request
{
    public class UpdateCampaignRequest : IRequest<ResponseBase<CampaignResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
