using CodeNet.Abstraction.Model;

namespace StokTakip.Product.Model;

public partial class CurrencyType : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SortName { get; set; }
    public string Symbol { get; set; }
}
