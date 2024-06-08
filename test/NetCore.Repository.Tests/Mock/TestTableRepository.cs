using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.EntityFramework.Tests.Mock.Model;

namespace NetCore.EntityFramework.Tests.Mock
{
    public class TestTableRepository : TracingRepository<TestTable>
    {
        public TestTableRepository(DbContext dbContext, IIdentityContext identityContext) : base(dbContext, identityContext)
        {
        }
    }
}
