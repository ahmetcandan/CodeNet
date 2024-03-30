using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.Identity.Model;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[Authorize(Roles = "admin")]
[ApiController]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IdentityRole>))]
    public async Task<IActionResult> Post([FromBody] CreateRoleModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Route("edit")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Put([FromBody] UpdateRoleModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditClaims([FromBody] RoleClaimsModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Delete([FromBody] DeleteRoleModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<RoleViewModel>>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetRoleQuery(), cancellationToken));
    }
}
