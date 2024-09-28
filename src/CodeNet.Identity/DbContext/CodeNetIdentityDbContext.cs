using CodeNet.Identity.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Identity;

public class CodeNetIdentityDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
}
