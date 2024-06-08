using Microsoft.EntityFrameworkCore;
using NetCore.EntityFramework.Tests.Mock.Model;

namespace NetCore.EntityFramework.Tests.Mock
{
    public class MockDbContext(DbContextOptions<DbContext> options) : DbContext(options)
    {
        public virtual DbSet<TestTable> TestTables { get; set; }
    }
}
