using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class DeleteParameterGroupHandler(ParametersDbContext dbContext, IIdentityContext identityContext) : IRequestHandler<DeleteParameterGroupRequest, ResponseBase<ParameterGroupResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterGroupResult>> Handle(DeleteParameterGroupRequest request, CancellationToken cancellationToken)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([request.ParameterGroupId], cancellationToken);
        if (parameterGroup is not null)
            _parameterGroupRepository.Remove(parameterGroup);
        else
            throw new ParameterException("PR001", $"Not found parameter group (Id: {request?.ParameterGroupId}).");
        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);

        return new ResponseBase<ParameterGroupResult>(parameterGroup.ToParameterGroupResult());
    }
}
