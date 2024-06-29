using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity;

public class CodeNetIdentityDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
{
}
