using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

    [HttpGet("{productId}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<ProductResponse>))]
    public async Task<IActionResult> Get(int productId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProductRequest { Id = productId }, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ResponseBase<ProductResponse>))]
    public async Task<IActionResult> Post(CreateProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(ResponseBase<ProductResponse>))]
    public async Task<IActionResult> Put(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(ResponseBase<ProductResponse>))]
    public async Task<IActionResult> Delete(int productId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new DeleteProductRequest { Id = productId }, cancellationToken));
    }
}
