using CodeNet.Core;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithSymmetricKey(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICodeNetContext codeNetContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManager(userManager, roleManager, codeNetContext, identityOptions)
{
    internal override JwtSecurityToken GetJwtSecurityToken(DateTime now, List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions.Value.IssuerSigningKey));

        return new JwtSecurityToken(
            issuer: identityOptions.Value.ValidIssuer,
            audience: identityOptions.Value.ValidAudience,
            expires: now.AddHours(5),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
    }
}
