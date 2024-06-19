using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;
using CodeNet.Identity.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CodeNet.Core.Security;

namespace CodeNet.Identity.Manager;

public class IdentityTokenManager(UserManager<IdentityUser> UserManager, RoleManager<IdentityRole> RoleManager, IOptions<IdentityConfig> JwtConfig) : IIdentityTokenManager
{
    public async Task<ResponseBase<TokenResponse>> GenerateToken(LoginModel model)
    {
        var now = DateTime.Now;
        var user = await UserManager.FindByNameAsync(model.Username);
        if (user is not null && await UserManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Email, user.Email),
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

            var rsa = AsymmetricKeyEncryption.CreateRSA("private_key.pem");

            var token = new JwtSecurityToken(
                issuer: JwtConfig.Value.ValidIssuer,
                audience: JwtConfig.Value.ValidAudience,
                expires: now.AddHours(JwtConfig.Value.ExpiryTime),
                claims: claims,
                signingCredentials: new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
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
}
