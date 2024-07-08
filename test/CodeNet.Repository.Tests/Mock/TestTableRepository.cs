using Microsoft.EntityFrameworkCore;
using CodeNet.EntityFramework.Tests.Mock.Model;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Core;

namespace CodeNet.EntityFramework.Tests.Mock
{
    public class TestTableRepository : TracingRepository<TestTable>
    {
        public TestTableRepository(DbContext dbContext, ICodeNetContext identityContext) : base(dbContext, identityContext)
        {
        }
    }
}
