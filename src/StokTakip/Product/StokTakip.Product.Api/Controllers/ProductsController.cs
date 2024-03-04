using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Product.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{productId}")]
        public async Task<ResponseBase<ProductResponse>> Get(int productId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetProductRequest { Id = productId }, cancellationToken);
        }

        [HttpPost]
        public async Task<ResponseBase<ProductResponse>> Post(CreateProductRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpPut]
        public async Task<ResponseBase<ProductResponse>> Put(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpDelete]
        public async Task<ResponseBase<ProductResponse>> Delete(int productId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new DeleteProductRequest { Id = productId }, cancellationToken);
        }
    }
}
