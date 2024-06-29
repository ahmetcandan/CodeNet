using CodeNet.MakerChecker;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters;

public class ParametersDbContext(DbContextOptions options) : MakerCheckerDbContext(options)
{
    public virtual DbSet<ParameterGroup> ParameterGroups { get; set; }
    public virtual DbSet<Parameter> Parameters { get; set; }
}