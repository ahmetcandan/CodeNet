using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerHistoryRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetContext identityContext) : TracingRepository<MakerCheckerHistory>(makerCheckerDbContext, identityContext)
{
}
