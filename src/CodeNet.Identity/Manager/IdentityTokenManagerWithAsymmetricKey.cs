using CodeNet.Core.Security;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithAsymmetricKey(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, CodeNetIdentityDbContext<IdentityUser, IdentityRole, string> dbContext, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManagerWithAsymmetricKey<IdentityUser>(userManager, roleManager, dbContext, identityOptions)
{
}

internal class IdentityTokenManagerWithAsymmetricKey<TUser>(UserManager<TUser> userManager, RoleManager<IdentityRole> roleManager, CodeNetIdentityDbContext<TUser, IdentityRole, string> dbContext, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManagerWithAsymmetricKey<TUser, IdentityRole>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<string>
{
}

internal class IdentityTokenManagerWithAsymmetricKey<TUser, TRole>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, CodeNetIdentityDbContext<TUser, TRole, string> dbContext, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManagerWithAsymmetricKey<TUser, TRole, string>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<string>
    where TRole : IdentityRole<string>
{
}

internal class IdentityTokenManagerWithAsymmetricKey<TUser, TRole, TKey>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, CodeNetIdentityDbContext<TUser, TRole, TKey> dbContext, IOptions<IdentitySettingsWithAsymmetricKey> identityOptions) : IdentityTokenManager<TUser, TRole, TKey>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    internal override RsaSecurityKey GetSecurityKey()
    {
        return new RsaSecurityKey(AsymmetricKeyEncryption.CreateRSA(identityOptions.Value.PrivateKeyPath));
    }
}
