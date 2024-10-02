using CodeNet.Core.Security;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithAsymmetricKey(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManager(userManager, roleManager, identityOptions)
{
    internal override RsaSecurityKey GetSecurityKey()
    {
        return new RsaSecurityKey(AsymmetricKeyEncryption.CreateRSA(identityOptions.Value.PrivateKeyPath));
    }
}
