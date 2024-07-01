using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerDefinitionRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetHttpContext identityContext) : TracingRepository<MakerCheckerDefinition>(makerCheckerDbContext, identityContext)
{
}
