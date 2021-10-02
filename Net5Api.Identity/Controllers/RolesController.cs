using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Net5Api.EntityFramework.Model;
using Net5Api.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthenticationWithSwagger.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public RolesController(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRoleModel model)
        {
            var roleExists = await roleManager.FindByNameAsync(model.Name);
            if (roleExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role already exists!" });

            IdentityRole role = new IdentityRole()
            {
                Name = model.Name,
                NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
                ? model.Name.Replace(" ", "").ToUpper()
                : model.NormalizedName
            };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role creation failed! Please check role details and try again." });

            return Ok(role);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RoleModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role not found!" });

            role.Name = model.Name;
            role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName) 
                ? model.Name.Replace(" ", "").ToUpper()
                : model.NormalizedName;
            var result = await roleManager.UpdateAsync(role);
            
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role creation failed! Please check role details and try again." });

            return Ok(new Response { Status = "Success", Message = "Role updated successfully!" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] RoleModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role not found!" });

            var result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role delete failed! Please check role details and try again." });

            return Ok(new Response { Status = "Success", Message = "Role deleted successfully!" });
        }
    }
}
