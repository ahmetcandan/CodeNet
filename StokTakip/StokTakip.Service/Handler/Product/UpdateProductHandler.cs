using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Abstraction;
using StokTakip.Contract.Request.Product;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service.Handler.Product
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, ResponseBase<ProductViewModel>>
    {
        private readonly IProductService _productService;

        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<ProductViewModel>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.UpdateProduct(new ProductViewModel 
            {
                Id = request.Id,
                Code = request.Code,
                Description = request.Description,
                Name = request.Name,
                Barcode = request.Barcode,
                CategoryId = request.CategoryId,
                TaxRate = request.TaxRate
            }, cancellationToken);
            return new ResponseBase<ProductViewModel>
            {
                Data = product,
                IsSuccessfull = true
            };
        }
    }
}
