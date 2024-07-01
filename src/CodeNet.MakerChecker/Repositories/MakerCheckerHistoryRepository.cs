using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerHistoryRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetHttpContext identityContext) : TracingRepository<MakerCheckerHistory>(makerCheckerDbContext, identityContext)
{
}
