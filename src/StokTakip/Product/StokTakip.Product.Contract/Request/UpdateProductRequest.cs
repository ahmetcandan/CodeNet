using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Response;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Product.Contract.Request;

public class UpdateProductRequest : IRequest<ResponseBase<ProductResponse>>
{
    public required int Id { get; set; }

    [MaxLength(50)]
    public required string Code { get; set; }

    [MaxLength(150)]
    public string? Description { get; set; }

    [MaxLength(150)]
    public required string Name { get; set; }

    public required int CategoryId { get; set; }

    public decimal TaxRate { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }
}
