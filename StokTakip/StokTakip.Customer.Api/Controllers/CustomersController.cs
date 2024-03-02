using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Contract.Request.Customer;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Customer.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{customerId}")]
        public async Task<ResponseBase<CustomerViewModel>> Get(int customerId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetCustomerRequest { Id = customerId }, cancellationToken);
        }

        [HttpPost]
        public async Task<ResponseBase<CustomerViewModel>> Post(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpPut]
        public async Task<ResponseBase<CustomerViewModel>> Put(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpDelete]
        public async Task<ResponseBase<CustomerViewModel>> Delete(int customerId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new DeleteCustomerRequest { Id = customerId }, cancellationToken);
        }
    }
}
