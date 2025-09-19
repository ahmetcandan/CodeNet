using CodeNet.Core.Context;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Repository.Tests.Mock.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Repository.Tests.Mock;

public class TestTracingRepository(DbContext dbContext, ICodeNetContext codeNetContext) : TracingRepository<TestTable>(dbContext, codeNetContext)
{
}

public class TestRepository(DbContext dbContext) : Repository<TestTable>(dbContext)
{
}
