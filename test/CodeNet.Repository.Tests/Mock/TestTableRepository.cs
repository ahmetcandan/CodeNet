using Microsoft.EntityFrameworkCore;
using CodeNet.Abstraction;
using CodeNet.EntityFramework.Tests.Mock.Model;

namespace CodeNet.EntityFramework.Tests.Mock
{
    public class TestTableRepository : TracingRepository<TestTable>
    {
        public TestTableRepository(DbContext dbContext, IIdentityContext identityContext) : base(dbContext, identityContext)
        {
        }
    }
}
