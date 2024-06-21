using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public class MakerCheckerFlowRepository(MakerCheckerDbContext makerCheckerDbContext, IIdentityContext identityContext) : TracingRepository<MakerCheckerFlow>(makerCheckerDbContext, identityContext), IMakerCheckerFlowRepository
{
}
