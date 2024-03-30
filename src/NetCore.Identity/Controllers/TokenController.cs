﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.Identity.Model;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(ResponseBase<TokenResponse>))]
    public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }
}
