using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Contract.Request;

public class GetCampaignRequest : IRequest<ResponseBase<CampaignResponse>>
{
    public required int Id { get; set; }
}
