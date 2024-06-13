using CodeNet.Abstraction.Model;

namespace StokTakip.Campaign.Model;

public class CampaignRequirement : IEntity
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public int? ProductId { get; set; }
    public int? CategoryId { get; set; }
    public decimal Amount { get; set; }
}
