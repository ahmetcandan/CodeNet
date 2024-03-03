using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Contract.Request
{
    public class CreateProductRequest : IRequest<ResponseBase<ProductResponse>>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal TaxRate { get; set; }
        public string Barcode { get; set; }
    }
}
