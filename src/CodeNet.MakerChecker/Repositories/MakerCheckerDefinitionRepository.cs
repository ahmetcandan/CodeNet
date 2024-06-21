using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public class MakerCheckerDefinitionRepository(MakerCheckerDbContext makerCheckerDbContext, IIdentityContext identityContext) : TracingRepository<MakerCheckerDefinition>(makerCheckerDbContext, identityContext), IMakerCheckerDefinitionRepository
{
}
