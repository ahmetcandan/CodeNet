using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerFlowRepository(MakerCheckerDbContext makerCheckerDbContext, IIdentityContext identityContext) : TracingRepository<MakerCheckerFlow>(makerCheckerDbContext, identityContext)
{
}
