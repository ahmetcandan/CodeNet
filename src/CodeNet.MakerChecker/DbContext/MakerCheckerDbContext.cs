using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker;

public class MakerCheckerDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<MakerCheckerDefinition> MakerCheckerDefinitions { get; set; }
    public virtual DbSet<MakerCheckerFlow> MakerCheckerFlows { get; set; }
    public virtual DbSet<MakerCheckerHistory> MakerCheckerHistories { get; set; }
    public virtual DbSet<MakerCheckerDraftEntity> MakerCheckerDraftEntities { get; set; }
}
