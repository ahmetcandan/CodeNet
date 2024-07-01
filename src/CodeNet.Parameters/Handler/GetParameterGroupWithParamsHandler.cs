using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class GetParameterGroupWithParamsHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<GetParameterGroupWithParamsRequest, ResponseBase<ParameterGroupWithParamsResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterGroupWithParamsResult>> Handle(GetParameterGroupWithParamsRequest request, CancellationToken cancellationToken)
    {
        ParameterGroupWithParamsResult? data;
        if (request.ParameterGroupId.HasValue)
            data = await _parameterGroupRepository.GetParameterGroupWithParams(request.ParameterGroupId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.ParameterGroupCode))
            data = await _parameterGroupRepository.GetParameterGroupWithParams(request.ParameterGroupCode, cancellationToken);
        else
            throw new ParameterException("PR004", "Invalid parameter");

        return new ResponseBase<ParameterGroupWithParamsResult>(data);
    }
}
