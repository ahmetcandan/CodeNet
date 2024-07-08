using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerDraftEntityRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetContext identityContext) : TracingRepository<MakerCheckerDraftEntity>(makerCheckerDbContext, identityContext)
{
}
