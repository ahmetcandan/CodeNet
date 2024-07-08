using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerDefinitionRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetContext identityContext) : TracingRepository<MakerCheckerDefinition>(makerCheckerDbContext, identityContext)
{
}
