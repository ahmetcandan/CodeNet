using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.EntityFramework.Model;
using NetCore.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountsController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("register")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("001", "User already exists!"));

        var user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded && model.Roles != null && model.Roles.Count > 0)
            await _userManager.AddToRolesAsync(user, model.Roles);
        return !result.Succeeded
            ? StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("003", "User creation failed! Please check user details and try again."))
            : (IActionResult)Ok(new ResponseBase("000", "User created successfully!"));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editroles")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditRoles([FromBody] UpdateUserRolesModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("002", "User not found!"));

        var currentRoles = await _userManager.GetRolesAsync(user);

        // delete roles
        await _userManager.RemoveFromRolesAsync(user, currentRoles.Where(c => !model.Roles.Any(r => r.Equals(c))));

        //add roles
        await _userManager.AddToRolesAsync(user, model.Roles.Where(r => !currentRoles.Any(c => c.Equals(r))));

        return Ok(new ResponseBase("000", "User updated roles successfully!"));
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("editcliams")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> EditCliams([FromBody] UpdateUserClaimsModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("002", "User not found!"));

        var currentClaims = await _userManager.GetClaimsAsync(user);

        // delete roles
        await _userManager.RemoveClaimsAsync(user, currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))));

        //add roles
        await _userManager.AddClaimsAsync(user, model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))));

        return Ok(new ResponseBase("000", "User updated claims successfully!"));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("getuser/{username}")]
    [ProducesResponseType(200, Type = typeof(ResponseBase<UserModel>))]
    public async Task<IActionResult> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("002", "User not found!"));

        var currentRoles = await _userManager.GetRolesAsync(user);
        return Ok(new ResponseBase<UserModel>(new UserModel()
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
        var users = _userManager.Users.ToList();

        var result = new List<UserModel>();
        foreach (var user in users)
            result.Add(new UserModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                Id = user.Id
            });

        return Ok(new ResponseBase<IEnumerable<UserModel>>(result));
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    [Route("removeuser")]
    [ProducesResponseType(200, Type = typeof(ResponseBase))]
    public async Task<IActionResult> RemoveUser([FromBody] RemoveUserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("002", "User not found!"));

        await _userManager.DeleteAsync(user);

        return Ok(new ResponseBase("000", "User removed successfully!"));
    }
}
