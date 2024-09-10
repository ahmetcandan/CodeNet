using System.ComponentModel.DataAnnotations;

namespace StokTakip.Product.Contract.Request;

public class CreateProductRequest
{
    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(150)]
    public string? Description { get; set; }

    [MaxLength(150)]
    public string Name { get; set; }

    public int CategoryId { get; set; }

    public decimal TaxRate { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }
}
