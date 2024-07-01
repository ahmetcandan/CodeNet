using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Attributes;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class GetParameterGroupHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<GetParameterGroupRequest, ResponseBase<ParameterGroupResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    [ParameterCache]
    public async Task<ResponseBase<ParameterGroupResult>> Handle(GetParameterGroupRequest request, CancellationToken cancellationToken)
    {
        var parameterGroup = request.ParameterGroupId.HasValue ?
                                await _parameterGroupRepository.GetAsync([request.ParameterGroupId.Value], cancellationToken)
                                : await _parameterGroupRepository.GetAsync(c => c.Code == request.ParameterGroupCode, cancellationToken);
        if (parameterGroup is null)
            return new ResponseBase<ParameterGroupResult>(null);

        return new ResponseBase<ParameterGroupResult>(parameterGroup.ToParameterGroupResult());
    }
}
