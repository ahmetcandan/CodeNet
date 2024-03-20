using Microsoft.EntityFrameworkCore;
using NetCore.Repository.Tests.Mock.Model;

namespace NetCore.Repository.Tests.Mock
{
    public class MockDbContext(DbContextOptions<DbContext> options) : DbContext(options)
    {
        public virtual DbSet<TestTable> TestTables { get; set; }
    }
}
