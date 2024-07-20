using Microsoft.EntityFrameworkCore;
using CodeNet.EntityFramework.Tests.Mock.Model;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Core;

namespace CodeNet.EntityFramework.Tests.Mock
{
    public class TestTracingRepository(DbContext dbContext, ICodeNetContext codeNetContext) : TracingRepository<TestTable>(dbContext, codeNetContext)
    {
    }

    public class TestRepository(DbContext dbContext) : Repository<TestTable>(dbContext)
    {

    }
}
