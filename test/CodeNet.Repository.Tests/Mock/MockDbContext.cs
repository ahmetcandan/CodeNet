using CodeNet.Repository.Tests.Mock.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Repository.Tests.Mock;

public class MockDbContext(DbContextOptions<DbContext> options) : DbContext(options)
{
    public virtual DbSet<TestTable> TestTables { get; set; }
}
