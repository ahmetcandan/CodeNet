using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Model;

public class Employee : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Code { get; set; }
}
