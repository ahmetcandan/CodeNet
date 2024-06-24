using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class MockMakerCheckerDbContext(DbContextOptions<MakerCheckerDbContext> options) : MakerCheckerDbContext(options)
{
    public DbSet<TestTable> TestTables { get; set; }
}
