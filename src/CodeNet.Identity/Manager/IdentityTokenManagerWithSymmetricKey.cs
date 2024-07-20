using CodeNet.Identity.Exception;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithSymmetricKey(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IIdentityTokenManager
{
    public async Task<TokenResponse> GenerateToken(LoginModel model)
    {
        var now = DateTime.Now;
        var user = await userManager.FindByNameAsync(model.Username);
        if (user is not null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Email, user.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

            claims.AddRange(await userManager.GetClaimsAsync(user));
            foreach (var roleName in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                var role = await roleManager.FindByNameAsync(roleName);
                claims.AddRange(await roleManager.GetClaimsAsync(role));
            }

            claims.Add(new Claim("LoginTime", now.ToString("O"), "DateTime[O]"));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions.Value.IssuerSigningKey));

            var token = new JwtSecurityToken(
                issuer: identityOptions.Value.ValidIssuer,
                audience: identityOptions.Value.ValidAudience,
                expires: now.AddHours(5),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


            return new TokenResponse
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
            };
        }
        throw new IdentityException("ID101", "Error: username or password incorrect.");
    }
}
