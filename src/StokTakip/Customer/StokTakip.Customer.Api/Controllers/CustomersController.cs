using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{customerId}")]
    [Cache(10)]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> Post(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.CreateCustomer(request, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> Put(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.UpdateCustomer(request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> Delete(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.DeleteCustomer(request.Id, cancellationToken));
    }
}
