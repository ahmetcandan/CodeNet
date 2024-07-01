using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class AddParameterGroupHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<AddParameterGroupRequest, ResponseBase<ParameterGroupResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterGroupResult>> Handle(AddParameterGroupRequest request, CancellationToken cancellationToken)
    {
        var parameterGroup = new ParameterGroup
        {
            Code = request.Code,
            ApprovalRequired = request.ApprovalRequired,
            Description = request.Description
        };
        var addResponse = await _parameterGroupRepository.AddAsync(parameterGroup, cancellationToken);
        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
        return new ResponseBase<ParameterGroupResult>(addResponse.ToParameterGroupResult());
    }
}
