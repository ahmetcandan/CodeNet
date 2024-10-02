using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithSymmetricKey(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManager(userManager, roleManager, identityOptions)
{
    internal override SymmetricSecurityKey GetSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityOptions.Value.IssuerSigningKey));
    }
}
