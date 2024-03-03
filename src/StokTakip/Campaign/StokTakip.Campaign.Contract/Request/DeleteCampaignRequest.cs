using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Contract.Request
{
    public class DeleteCampaignRequest : IRequest<ResponseBase<CampaignResponse>>
    {
        public int Id { get; set; }
    }
}
