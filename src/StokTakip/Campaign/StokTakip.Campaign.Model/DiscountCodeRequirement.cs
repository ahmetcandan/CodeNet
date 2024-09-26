namespace StokTakip.Campaign.Model;

public partial class DiscountCodeRequirement
{
    public int Id { get; set; }
    public int DiscountCodeId { get; set; }
    public int? ProductId { get; set; }
    public int? CategoryId { get; set; }
    public decimal Amount { get; set; }
}
