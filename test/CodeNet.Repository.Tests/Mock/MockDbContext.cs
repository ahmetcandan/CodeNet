using Microsoft.EntityFrameworkCore;
using CodeNet.EntityFramework.Tests.Mock.Model;

namespace CodeNet.EntityFramework.Tests.Mock
{
    public class MockDbContext(DbContextOptions<DbContext> options) : DbContext(options)
    {
        public virtual DbSet<TestTable> TestTables { get; set; }
    }
}
