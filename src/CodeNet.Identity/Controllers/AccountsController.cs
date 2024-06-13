using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("register")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editroles")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditClaims([FromBody] UpdateUserClaimsModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("getuser/{username}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<UserModel>))]
    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetUserQuery(username), cancellationToken));
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    [Route("removeuser")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(model, cancellationToken));
    }
}
