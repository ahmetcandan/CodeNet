using CodeNet.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Product.Model;

public class Product : TracingEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Code { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [MaxLength(150)]
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public decimal TaxRate { get; set; }
}
