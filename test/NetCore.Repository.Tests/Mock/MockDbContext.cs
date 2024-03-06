using Microsoft.EntityFrameworkCore;
using NetCore.Repository.Tests.Mock.Model;

namespace NetCore.Repository.Tests.Mock
{
    public class MockDbContext : DbContext
    {

        public MockDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<TestTable> TestTables { get; set; }
    }
}
