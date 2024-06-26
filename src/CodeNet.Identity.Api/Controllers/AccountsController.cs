using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeNet.Core.Models;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Controllers;

[Route("[controller]")]
//[Authorize(Roles = "admin")]
[ApiController]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Route("editroles")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditClaims([FromBody] UpdateUserClaimsModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpGet]
    [Route("getuser/{username}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<UserModel>))]
    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetUserQuery(username), cancellationToken));
    }

    [HttpDelete]
    [Route("removeuser")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }
}
