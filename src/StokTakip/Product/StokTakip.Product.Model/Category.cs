using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Product.Model;

public class Category : TracingEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Code { get; set; }

    public int? ParentId { get; set; }
}
