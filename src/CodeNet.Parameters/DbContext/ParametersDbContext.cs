using CodeNet.MakerChecker.DbContext;
using CodeNet.Parameters.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Parameters.DbContext;

public class ParametersDbContext(DbContextOptions options) : MakerCheckerDbContext(options)
{
    public virtual DbSet<ParameterGroup> ParameterGroups { get; set; }
    public virtual DbSet<Parameter> Parameters { get; set; }
}