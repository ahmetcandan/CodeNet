using CodeNet.Core;
using CodeNet.MakerChecker.Repositories;
using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Repositories;

internal class ParameterMakerCheckerRepository(ParametersDbContext dbContext, IIdentityContext identityContext) : MakerCheckerRepository<Parameter>(dbContext, identityContext), IParameterRepository
{
}
