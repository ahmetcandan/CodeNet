using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Model;

public class Customer : TracingEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(10)]
    public string No { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }
}
