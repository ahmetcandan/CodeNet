using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Contract.Request
{
    public class GetCampaignRequest : IRequest<ResponseBase<CampaignResponse>>
    {
        public int Id { get; set; }
    }
}
