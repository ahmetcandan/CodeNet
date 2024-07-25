using CodeNet.EntityFramework.Models;

namespace StokTakip.Campaign.Model;

public partial class DiscountCodeUsedHistory
{
    public int DiscountCodeId { get; set; }
    public int SalesOrderId { get; set; }
    public DateTime UsedDate { get; set; }
    public bool IsCancel { get; set; }
}
