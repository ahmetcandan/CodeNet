using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction.Model;
using NetCore.Identity.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[Authorize(Roles = "admin")]
[ApiController]
public class RolesController(RoleManager<IdentityRole> roleManager) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IdentityRole>))]
    public async Task<IActionResult> Post([FromBody] CreateRoleModel model)
    {
        var roleExists = await roleManager.FindByNameAsync(model.Name);
        if (roleExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("011", "Role already exists!"));

        IdentityRole role = new()
        {
            Name = model.Name,
            NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName
        };
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("013", "Role creation failed! Please check role details and try again."));

        return Ok(new ResponseBase<IdentityRole>(role));
    }

    [HttpPut]
    [Route("edit")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Put([FromBody] RoleModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id);
        if (role == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("012", "Role not found!"));

        role.Name = model.Name;
        role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName;
        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("014", "Role update failed! Please check role details and try again."));

        return Ok(new ResponseBase("000", "Role updated successfully!"));
    }

    [HttpPut]
    [Route("editclaims")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditClaims([FromBody] RoleClaimsModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id);
        if (role == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("012", "Role not found!"));

        var currentClaims = await roleManager.GetClaimsAsync(role);

        // delete roles
        foreach (var claim in currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))))
            await roleManager.RemoveClaimAsync(role, claim);

        //add roles
        foreach (var claim in model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))))
            await roleManager.AddClaimAsync(role, claim);

        return Ok(new ResponseBase("000", "Role claims updated successfully!"));
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Delete([FromBody] RoleModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id);
        if (role == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("012", "Role not found!"));

        var result = await roleManager.DeleteAsync(role);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("015", "Role delete failed! Please check role details and try again."));

        return Ok(new ResponseBase("000", "Role deleted successfully!"));
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<IdentityRole>>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(new ResponseBase<IEnumerable<IdentityRole>>(await roleManager.Roles.ToListAsync(cancellationToken)));
    }
}
