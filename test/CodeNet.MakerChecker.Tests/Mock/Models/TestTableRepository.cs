using CodeNet.Core;
using CodeNet.MakerChecker.Repositories;

namespace CodeNet.MakerChecker.Tests.Mock.Models;

public class TestTableRepository(MakerCheckerDbContext dbContext, ICodeNetHttpContext identityContext) : MakerCheckerRepository<TestTable>(dbContext, identityContext)
{
}
