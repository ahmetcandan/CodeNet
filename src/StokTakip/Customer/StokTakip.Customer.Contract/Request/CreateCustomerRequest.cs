using System.ComponentModel.DataAnnotations;

namespace StokTakip.Customer.Contract.Request;

public class CreateCustomerRequest
{
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(10)]
    public string Number { get; set; }

    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(150)]
    public string? Description { get; set; }
}
