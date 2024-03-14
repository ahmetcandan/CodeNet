using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[Authorize(Roles = "admin")]
[ApiController]
public class RolesController(IIdentityRoleManager IdentityRoleManager) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IdentityRole>))]
    public async Task<IActionResult> Post([FromBody] CreateRoleModel model)
    {
        return Ok(await IdentityRoleManager.CreateRole(model));
    }

    [HttpPut]
    [Route("edit")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Put([FromBody] RoleModel model)
    {
        return Ok(await IdentityRoleManager.EditRole(model));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditClaims([FromBody] RoleClaimsModel model)
    {
        return Ok(await IdentityRoleManager.EditRoleClaims(model));
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Delete([FromBody] RoleModel model)
    {
        return Ok(await IdentityRoleManager.DeleteRole(model));
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<IdentityRole>>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await IdentityRoleManager.GetRoles(cancellationToken));
    }
}
