using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class MockMakerCheckerDbContext(DbContextOptions options) : MakerCheckerDbContext(options)
{
    public DbSet<TestTable> TestTables { get; set; }
}
