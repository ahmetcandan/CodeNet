using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountsController(IIdentityUserManager IdentityUserManager) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("register")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
    {
        return Ok(await IdentityUserManager.CreateUser(model));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editroles")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model)
    {
        return Ok(await IdentityUserManager.EditUserRoles(model));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editcliams")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditCliams([FromBody] UpdateUserClaimsModel model)
    {
        return Ok(await IdentityUserManager.EditUserClaims(model));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("getuser/{username}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<UserModel>))]
    public async Task<IActionResult> GetUser(string username)
    {
        return Ok(await IdentityUserManager.GetUser(username));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("getallusers")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<UserModel>>))]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await IdentityUserManager.GetAllUsers());
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    [Route("removeuser")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model)
    {
        return Ok(await IdentityUserManager.RemoveUser(model));
    }
}
