namespace StokTakip.Campaign.Model;

public partial class GiftCardHistory
{
    public int GiftCardId { get; set; }
    public int SalesOrderId { get; set; }
    public decimal UsedAmount { get; set; }
    public DateTime UsedDate { get; set; }
    public bool IsCancel { get; set; }
}
