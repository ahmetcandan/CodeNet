using Microsoft.AspNetCore.Mvc;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("{productId}")]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    public async Task<IActionResult> Get(int productId, CancellationToken cancellationToken)
    {
        return Ok(await productService.GetProduct(productId, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    public async Task<IActionResult> Post(CreateProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await productService.CreateProduct(request, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    public async Task<IActionResult> Put(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await productService.UpdateProduct(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    public async Task<IActionResult> Delete(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await productService.DeleteProduct(request.Id, cancellationToken));
    }
}
