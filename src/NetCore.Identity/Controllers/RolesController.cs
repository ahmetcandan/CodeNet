using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Core.Models;
using NetCore.Identity.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<IdentityRole>))]
        public async Task<IActionResult> Post([FromBody] CreateRoleModel model)
        {
            var roleExists = await _roleManager.FindByNameAsync(model.Name);
            if (roleExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase<IdentityRole>.SetStatusCode(500, "Role already exists!"));

            IdentityRole role = new()
            {
                Name = model.Name,
                NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
                ? model.Name.Replace(" ", "").ToUpper()
                : model.NormalizedName
            };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase<IdentityRole>.SetStatusCode(500, "Role creation failed! Please check role details and try again."));

            return Ok(ResponseBase<IdentityRole>.SetData(role));
        }

        [HttpPut]
        [Route("edit")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> Put([FromBody] RoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "Role not found!"));

            role.Name = model.Name;
            role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
                ? model.Name.Replace(" ", "").ToUpper()
                : model.NormalizedName;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "Role update failed! Please check role details and try again."));

            return Ok(ResponseBase.SetStatusCode(200, "Role updated successfully!"));
        }

        [HttpPut]
        [Route("editclaims")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> EditClaims([FromBody] RoleClaimsModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "Role not found!"));

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            // delete roles
            foreach (var claim in currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))))
                await _roleManager.RemoveClaimAsync(role, claim);

            //add roles
            foreach (var claim in model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))))
                await _roleManager.AddClaimAsync(role, claim);

            return Ok(ResponseBase.SetStatusCode(200, "Role claims updated successfully!"));
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> Delete([FromBody] RoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "Role not found!"));

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "Role delete failed! Please check role details and try again."));

            return Ok(ResponseBase.SetStatusCode(200, "Role deleted successfully!"));
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<IdentityRole>>))]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(ResponseBase<IEnumerable<IdentityRole>>.SetData(await _roleManager.Roles.ToListAsync(cancellationToken)));
        }
    }
}
