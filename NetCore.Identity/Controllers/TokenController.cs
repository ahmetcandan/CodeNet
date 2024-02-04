using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCore.Core.Models;
using NetCore.EntityFramework.Model;
using NetCore.Identity.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public TokenController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseBase<TokenResponse>))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var now = DateTime.Now;
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var claims = new List<Claim>
                    {
                        new Claim("Username", user.UserName),
                        new Claim("UserId", user.Id),
                        new Claim("Email", user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    claims.AddRange(await userManager.GetClaimsAsync(user));
                    foreach (var roleName in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleName));
                        var role = await roleManager.FindByNameAsync(roleName);
                        claims.AddRange(await roleManager.GetClaimsAsync(role));
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


                    return Ok(ResponseBase<TokenResponse>.SetData(new TokenResponse
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
                return Ok(ResponseBase<TokenResponse>.SetStatusCode(500, "Error: username or password incorrect."));
            }
            catch
            {
                return Ok(ResponseBase<TokenResponse>.SetStatusCode(500, "Error"));
            }
        }
    }
}
