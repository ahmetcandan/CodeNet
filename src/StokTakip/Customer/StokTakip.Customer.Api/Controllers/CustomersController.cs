using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using StokTakip.Customer.Model;
using StokTakip.Customer.Service.QueueService;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [Authorize]
    [HttpGet("{customerId}")]
    [Cache(10)]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        var st = new StackTrace();
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]

    public async Task<IActionResult> Post(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.CreateCustomer(request, cancellationToken));
    }

    [HttpPut("{customerId}")]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> Put(int customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.UpdateCustomer(customerId, request, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> Delete(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        return Ok(await customerService.DeleteCustomer(request.Id, cancellationToken));
    }
}

[ApiController]
[Route("[controller]")]
public class MongoController(IKeyValueRepository keyValueRepository, IAKeyValueRepository aKeyValueRepository, IBKeyValueRepository bKeyValueRepository) : ControllerBase
{
    [HttpGet("A")]
    public async Task<IActionResult> GetA(Guid id, CancellationToken cancellationToken)
    {
        var x = await aKeyValueRepository.GetByIdAsync(c => c._id == id, cancellationToken);
        return Ok(x);
    }

    [HttpGet("B")]
    public async Task<IActionResult> GetB(Guid id, CancellationToken cancellationToken)
    {
        var x = await bKeyValueRepository.GetByIdAsync(c => c._id == id, cancellationToken);
        return Ok(x);
    }
    [HttpGet]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var x = await keyValueRepository.GetByIdAsync(c => c._id == id, cancellationToken);
        return Ok(x);
    }
}

[ApiController]
[Route("[controller]")]
public class RabbitMQController(ProducerServiceA producerServiceA, ProducerServiceB producerServiceB) : ControllerBase
{
    [HttpPost("A")]
    public async Task<IActionResult> SendA(QueueData data, CancellationToken cancellationToken)
    {
        return Ok(producerServiceA.Publish(data));
    }

    [HttpPost("B")]
    public async Task<IActionResult> SendB(QueueData data, CancellationToken cancellationToken)
    {
        return Ok(producerServiceB.Publish(data));
    }
}

[ApiController]
[Route("[controller]")]
public class StackExchangeController(RedisProducerServiceA producerServiceA, RedisProducerServiceB producerServiceB) : ControllerBase
{
    [HttpPost("A")]
    public async Task<IActionResult> SendA(QueueData data, CancellationToken cancellationToken)
    {
        return Ok(await producerServiceA.PublishAsync(data));
    }

    [HttpPost("B")]
    public async Task<IActionResult> SendB(QueueData data, CancellationToken cancellationToken)
    {
        return Ok(await producerServiceB.PublishAsync(data));
    }
}

public class TestModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public override string ToString()
    {
        return $"{{Name: {Name}, Date: {Date}}}";
    }
}
