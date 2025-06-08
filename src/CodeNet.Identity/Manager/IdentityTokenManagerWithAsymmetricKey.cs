using CodeNet.Core.Security;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithAsymmetricKey<TUser>(UserManager<TUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManager<TUser>(userManager, roleManager, identityOptions)
    where TUser : ApplicationUser
{
    internal override RsaSecurityKey GetSecurityKey()
    {
        return new RsaSecurityKey(AsymmetricKeyEncryption.CreateRSA(identityOptions.Value.PrivateKeyPath));
    }
}
