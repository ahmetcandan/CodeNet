using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(IMediator mediator, ILogger<CustomersController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{customerId}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CustomerResponse>))]
    public async Task<IActionResult> Get(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCustomerRequest { Id = customerId }, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CustomerResponse>))]
    public async Task<IActionResult> Post(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CustomerResponse>))]
    public async Task<IActionResult> Put(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(ResponseBase<CustomerResponse>))]
    public async Task<IActionResult> Delete(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new DeleteCustomerRequest { Id = customerId }, cancellationToken));
    }
}
