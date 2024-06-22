using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker;

public class MakerCheckerDbContext(DbContextOptions<MakerCheckerDbContext> options) : DbContext(options)
{
    public virtual DbSet<MakerCheckerDefinition> MakerCheckerDefinitions { get; set; }
    public virtual DbSet<MakerCheckerFlow> MakerCheckerFlows { get; set; }
    public virtual DbSet<MakerCheckerHistory> MakerCheckerHistories { get; set; }
}
