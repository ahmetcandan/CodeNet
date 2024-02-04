using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Core.Models;
using NetCore.EntityFramework.Model;
using NetCore.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("register")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "User already exists!"));

            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded && model.Roles != null && model.Roles.Count > 0)
                await userManager.AddToRolesAsync(user, model.Roles);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "User creation failed! Please check user details and try again."));

            return Ok(ResponseBase.SetStatusCode(200, "User created successfully!"));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("editroles")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "User not found!"));

            var currentRoles = await userManager.GetRolesAsync(user);

            // delete roles
            await userManager.RemoveFromRolesAsync(user, currentRoles.Where(c => !model.Roles.Any(r => r.Equals(c))));

            //add roles
            await userManager.AddToRolesAsync(user, model.Roles.Where(r => !currentRoles.Any(c => c.Equals(r))));

            return Ok(ResponseBase.SetStatusCode(200, "User updated roles successfully!"));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("editcliams")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> EditCliams([FromBody] UpdateUserClaimsModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "User not found!"));

            var currentClaims = await userManager.GetClaimsAsync(user);

            // delete roles
            await userManager.RemoveClaimsAsync(user, currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))));

            //add roles
            await userManager.AddClaimsAsync(user, model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))));

            return Ok(ResponseBase.SetStatusCode(200, "User updated claims successfully!"));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("getuser/{username}")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<UserModel>))]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase<UserModel>.SetStatusCode(500, "User not found!"));

            var currentRoles = await userManager.GetRolesAsync(user);
            return Ok(ResponseBase<UserModel>.SetData(new UserModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = currentRoles,
                Id = user.Id
            }));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("getallusers")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<IEnumerable<UserModel>>))]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = userManager.Users.ToList();

            var result = new List<UserModel>();
            foreach (var user in users)
                result.Add(new UserModel()
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = await userManager.GetRolesAsync(user),
                    Id = user.Id
                });

            return Ok(ResponseBase<IEnumerable<UserModel>>.SetData(result));
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("removeuser")]
        [ProducesResponseType(200, Type = typeof(ResponseBase))]
        public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseBase.SetStatusCode(500, "User not found!"));

            await userManager.DeleteAsync(user);

            return Ok(ResponseBase.SetStatusCode(200, "User removed successfully!"));
        }
    }
}
