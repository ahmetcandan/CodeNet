using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Attributes;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class GetParameterHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<GetParameterRequest, ResponseBase<ParameterResult>>
{
    private readonly ParameterTracingRepository _parameterRepository = new(dbContext, identityContext);

    [ParameterCache]
    public async Task<ResponseBase<ParameterResult>> Handle(GetParameterRequest request, CancellationToken cancellationToken)
    {
        var parameter = await _parameterRepository.GetAsync([request.ParameterId], cancellationToken);
        if (parameter is null)
            return new ResponseBase<ParameterResult>(null);

        return new ResponseBase<ParameterResult>(parameter.ToParameterResult());
    }
}
