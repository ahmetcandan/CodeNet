using CodeNet.Core.Context;

namespace CodeNet.Parameters.Repositories;

internal class ParameterRepositoryResolver(ParametersDbContext dbContext, ICodeNetContext identityContext)
{
    private readonly ParameterMakerCheckerRepository _parameterMakerCheckerRepository = new(dbContext, identityContext);
    private readonly ParameterTracingRepository _parameterTracingRepository = new(dbContext, identityContext);

    public IParameterRepository GetParameterRepository(bool approvelRequired) => approvelRequired ? _parameterMakerCheckerRepository : _parameterTracingRepository;
}
