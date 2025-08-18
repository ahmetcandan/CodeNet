using CodeNet.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity.DbContext;

public class CodeNetIdentityDbContext(DbContextOptions options) : CodeNetIdentityDbContext<IdentityUser>(options)
{
}

public class CodeNetIdentityDbContext<TUser>(DbContextOptions options) : CodeNetIdentityDbContext<TUser, IdentityRole>(options)
    where TUser : IdentityUser
{
}

public class CodeNetIdentityDbContext<TUser, TRole>(DbContextOptions options) : CodeNetIdentityDbContext<TUser, TRole, string>(options)
    where TUser : IdentityUser
    where TRole : IdentityRole
{
}

public class CodeNetIdentityDbContext<TUser, TRole, TKey>(DbContextOptions options) : IdentityDbContext<TUser, TRole, TKey>(options)
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// User Refresh Tokens
    /// </summary>
    public virtual DbSet<UserRefreshToken<TKey>> UserRefreshTokens { get; set; } = default!;
}
