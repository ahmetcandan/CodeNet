using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Product.Api.Controllers
{
    [Authorize(Roles = "product")]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId}")]
        public async Task<ProductViewModel> Get(int productId, CancellationToken cancellationToken)
        {
            return await _productService.GetProduct(productId, cancellationToken);
        }

        [HttpPost]
        public async Task<ProductViewModel> Post(ProductViewModel product, CancellationToken cancellationToken)
        {
            return await _productService.CreateProduct(product, cancellationToken);
        }

        [HttpPut]
        public async Task<ProductViewModel> Put(ProductViewModel product, CancellationToken cancellationToken)
        {
            return await _productService.UpdateProduct(product, cancellationToken);
        }

        [HttpDelete]
        public async Task<ProductViewModel> Delete(int productId, CancellationToken cancellationToken)
        {
            return await _productService.DeleteProduct(productId, cancellationToken);
        }
    }
}
