using CodeNet.Core;
using CodeNet.Core.Security;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithAsymmetricKey(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICodeNetContext codeNetContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManager(userManager, roleManager, codeNetContext, identityOptions)
{
    internal override JwtSecurityToken GetJwtSecurityToken(DateTime now, List<Claim> claims)
    {
        var rsa = AsymmetricKeyEncryption.CreateRSA(identityOptions.Value.IssuerSigningKey);

        return new JwtSecurityToken(
            issuer: identityOptions.Value.ValidIssuer,
            audience: identityOptions.Value.ValidAudience,
            expires: now.AddHours(identityOptions.Value.ExpiryTime),
            claims: claims,
            signingCredentials: new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            );
    }
}
