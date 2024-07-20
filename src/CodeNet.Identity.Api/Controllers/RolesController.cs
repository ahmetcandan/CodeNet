using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Identity.Api.Controllers;

[Route("[controller]")]
[Authorize(Roles = "admin")]
[ApiController]
public class RolesController(IIdentityRoleManager roleManager) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(IdentityRole))]
    public async Task<IActionResult> Post([FromBody] CreateRoleModel model)
    {
        return Ok(await roleManager.CreateRole(model));
    }

    [HttpPut]
    [Route("edit")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> Put([FromBody] UpdateRoleModel model)
    {
        return Ok(await roleManager.EditRole(model));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> EditClaims([FromBody] RoleClaimsModel model)
    {
        return Ok(await roleManager.EditRoleClaims(model));
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> Delete([FromBody] DeleteRoleModel model)
    {
        return Ok(await roleManager.DeleteRole(model));
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RoleViewModel>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await roleManager.GetRoles(cancellationToken));
    }
}
