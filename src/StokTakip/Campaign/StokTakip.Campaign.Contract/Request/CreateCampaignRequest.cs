using MediatR;
using CodeNet.Abstraction.Model;
using StokTakip.Campaign.Contract.Response;

namespace StokTakip.Campaign.Contract.Request;

public class CreateCampaignRequest : IRequest<ResponseBase<CampaignResponse>>
{
    public required string Name { get; set; }
}
