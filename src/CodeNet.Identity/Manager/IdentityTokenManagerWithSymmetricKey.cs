using CodeNet.Identity.DbContext;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CodeNet.Identity.Manager;

internal class IdentityTokenManagerWithSymmetricKey(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, CodeNetIdentityDbContext<IdentityUser, IdentityRole, string> dbContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManagerWithSymmetricKey<IdentityUser>(userManager, roleManager, dbContext, identityOptions)
{
}

internal class IdentityTokenManagerWithSymmetricKey<TUser>(UserManager<TUser> userManager, RoleManager<IdentityRole> roleManager, CodeNetIdentityDbContext<TUser, IdentityRole, string> dbContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManagerWithSymmetricKey<TUser, IdentityRole>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<string>
{
}

internal class IdentityTokenManagerWithSymmetricKey<TUser, TRole>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, CodeNetIdentityDbContext<TUser, TRole, string> dbContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManagerWithSymmetricKey<TUser, TRole, string>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<string>
    where TRole : IdentityRole<string>
{
}

internal class IdentityTokenManagerWithSymmetricKey<TUser, TRole, TKey>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, CodeNetIdentityDbContext<TUser, TRole, TKey> dbContext, IOptions<IdentitySettingsWithSymmetricKey> identityOptions) : IdentityTokenManager<TUser, TRole, TKey>(userManager, roleManager, dbContext, identityOptions)
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    internal override SymmetricSecurityKey GetSecurityKey() => new(Encoding.UTF8.GetBytes(identityOptions.Value.IssuerSigningKey));
}
