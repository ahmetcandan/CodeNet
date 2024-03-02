using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Model;

namespace StokTakip.Contract.Request.Product
{
    public class UpdateProductRequest : IRequest<ResponseBase<ProductViewModel>>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal TaxRate { get; set; }
        public string Barcode { get; set; }
    }
}
