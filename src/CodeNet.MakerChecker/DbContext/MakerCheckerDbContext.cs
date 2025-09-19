using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.DbContext;

public class MakerCheckerDbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public virtual DbSet<MakerCheckerFlow> MakerCheckerFlows { get; set; }
    public virtual DbSet<MakerCheckerHistory> MakerCheckerHistories { get; set; }
}
