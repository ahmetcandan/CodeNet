using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Attributes;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class GetParametersHandler(ParametersDbContext dbContext, IIdentityContext identityContext) : IRequestHandler<GetParametersRequest, ResponseBase<List<ParameterListItemResult>>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    [ParameterCache]
    public async Task<ResponseBase<List<ParameterListItemResult>>> Handle(GetParametersRequest request, CancellationToken cancellationToken)
    {
        List<ParameterListItemResult> result;
        if (request.ParameterGroupId.HasValue)
            result = await _parameterGroupRepository.GetParametersAsync(request.ParameterGroupId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.ParameterGroupCode))
            result = await _parameterGroupRepository.GetParametersAsync(request.ParameterGroupCode, cancellationToken);
        else
            throw new ParameterException("PR003", "Id or Code cannot be null.");

        return new ResponseBase<List<ParameterListItemResult>>(result);
    }
}
