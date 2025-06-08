using CodeNet.Identity.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity;

public class CodeNetIdentityDbContext(DbContextOptions options) : CodeNetIdentityDbContext<ApplicationUser>(options)
{
}

public class CodeNetIdentityDbContext<TUser>(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    where TUser : ApplicationUser
{
}
