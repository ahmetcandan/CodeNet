﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Customer.Api.Controllers
{
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
        public async Task<ResponseBase<CustomerResponse>> Get(int customerId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetCustomerRequest { Id = customerId }, cancellationToken);
        }

        [HttpPost]
        public async Task<ResponseBase<CustomerResponse>> Post(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpPut]
        public async Task<ResponseBase<CustomerResponse>> Put(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(request, cancellationToken);
        }

        [HttpDelete]
        public async Task<ResponseBase<CustomerResponse>> Delete(int customerId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new DeleteCustomerRequest { Id = customerId }, cancellationToken);
        }
    }
}
