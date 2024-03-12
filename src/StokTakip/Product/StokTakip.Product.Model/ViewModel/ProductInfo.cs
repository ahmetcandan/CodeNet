namespace StokTakip.Product.Model.ViewModel;

public class ProductInfo
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int CategoryId { get; set; }
    public decimal TaxRate { get; set; }
    public required string CategoryName { get; set; }
    public required string CategoryCode { get; set; }
}
