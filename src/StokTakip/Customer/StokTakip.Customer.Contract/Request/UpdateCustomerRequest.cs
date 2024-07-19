using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Contract.Request;

public class UpdateCustomerRequest
{
    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(10)]
    public required string No { get; set; }

    [MaxLength(50)]
    public required string Code { get; set; }

    [MaxLength(150)]
    public string? Description { get; set; }
}
