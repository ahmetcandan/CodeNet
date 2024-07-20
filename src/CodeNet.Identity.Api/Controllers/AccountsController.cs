using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Identity.Api.Controllers;

[Route("[controller]")]
[Authorize(Roles = "admin")]
[ApiController]
public class AccountsController(IIdentityUserManager userManager) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
    {
        return Ok(await userManager.CreateUser(model));
    }

    [HttpPut]
    [Route("editroles")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model)
    {
        return Ok(await userManager.EditUserRoles(model));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> EditClaims([FromBody] UpdateUserClaimsModel model)
    {
        return Ok(await userManager.EditUserClaims(model));
    }

    [HttpGet]
    [Route("getuser/{username}")]
    [ProducesResponseType(200, Type = typeof(UserModel))]
    public async Task<IActionResult> GetUser(string username)
    {
        return Ok(await userManager.GetUser(username));
    }

    [HttpDelete]
    [Route("removeuser")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model)
    {
        return Ok(await userManager.RemoveUser(model));
    }
}
