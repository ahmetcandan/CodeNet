using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity.DbContext;

public class CodeNetIdentityDbContext(DbContextOptions<CodeNetIdentityDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
}
