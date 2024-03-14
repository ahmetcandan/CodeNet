using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using NetCore.Abstraction.Model;
using NetCore.EntityFramework.Model;
using NetCore.ExceptionHandling;
using NetCore.Identity.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCore.Identity.Manager;

public class IdentityTokenManager(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager, IOptions<JwtConfig> JwtConfig) : IIdentityTokenManager
{
    public async Task<ResponseBase> GenerateToken(LoginModel model)
    {
        try
        {
            var now = DateTime.Now;
            var user = await UserManager.FindByNameAsync(model.Username);
            if (user != null && await UserManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await UserManager.GetRolesAsync(user);

                var claims = new List<Claim>
                    {
                        new("Username", user.UserName),
                        new("UserId", user.Id),
                        new("Email", user.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                claims.AddRange(await UserManager.GetClaimsAsync(user));
                foreach (var roleName in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                    var role = await RoleManager.FindByNameAsync(roleName);
                    claims.AddRange(await RoleManager.GetClaimsAsync(role));
                }

                claims.Add(new Claim("LoginTime", now.ToString("O"), "DateTime[O]"));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Value.Secret));

                var token = new JwtSecurityToken(
                    issuer: JwtConfig.Value.ValidIssuer,
                    audience: JwtConfig.Value.ValidAudience,
                    expires: now.AddHours(JwtConfig.Value.ExpiryTime),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                return new ResponseBase<TokenResponse>(new TokenResponse
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
                });
            }
            throw new UserLevelException("101", "Error: username or password incorrect.");
        }
        catch
        {
            throw new UserLevelException("102", "Unexpected error!");
        }
    }
}
