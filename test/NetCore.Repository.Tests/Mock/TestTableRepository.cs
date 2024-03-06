using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Repository.Tests.Mock.Model;

namespace NetCore.Repository.Tests.Mock
{
    public class TestTableRepository : BaseRepository<TestTable>
    {
        public TestTableRepository(DbContext dbContext, IIdentityContext identityContext) : base(dbContext, identityContext)
        {
        }
    }
}
