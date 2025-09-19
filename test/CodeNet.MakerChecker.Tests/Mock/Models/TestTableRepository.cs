using CodeNet.Core.Context;
using CodeNet.MakerChecker.DbContext;
using CodeNet.MakerChecker.Repositories;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class TestTableRepository(MakerCheckerDbContext dbContext, ICodeNetContext identityContext) : MakerCheckerRepository<TestTable>(dbContext, identityContext)
{
}
