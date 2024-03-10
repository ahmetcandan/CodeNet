using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCore.Abstraction.Model;
using NetCore.EntityFramework.Model;
using NetCore.Identity.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ResponseBase<TokenResponse>))]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var now = DateTime.Now;
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                    {
                        new("Username", user.UserName),
                        new("UserId", user.Id),
                        new("Email", user.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                claims.AddRange(await _userManager.GetClaimsAsync(user));
                foreach (var roleName in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                    var role = await _roleManager.FindByNameAsync(roleName);
                    claims.AddRange(await _roleManager.GetClaimsAsync(role));
                }

                claims.Add(new Claim("LoginTime", now.ToString("O"), "DateTime[O]"));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: now.AddHours(5),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                return Ok(new ResponseBase<TokenResponse>(new TokenResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    CreatedDate = DateTime.UtcNow,
                    Claims = from c in claims
                             select new ClaimResponse
                             {
                                 Type = c.Type,
                                 Value = c.Value,
                                 ValueType = c.ValueType
                             }
                }));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("101", "Error: username or password incorrect."));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase("102", "Error"));
        }
    }
}
