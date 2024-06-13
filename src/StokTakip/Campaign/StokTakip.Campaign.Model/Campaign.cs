using CodeNet.Abstraction.Model;

namespace StokTakip.Campaign.Model;

public class Campaign : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public bool AmountOrRate { get; set; }
    public int CurrencyTypeId { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
