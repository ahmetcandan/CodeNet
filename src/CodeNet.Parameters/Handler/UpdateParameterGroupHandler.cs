using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class UpdateParameterGroupHandler(ParametersDbContext dbContext, IIdentityContext identityContext) : IRequestHandler<UpdateParameterGroupRequest, ResponseBase<ParameterGroupResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterGroupResult>> Handle(UpdateParameterGroupRequest request, CancellationToken cancellationToken)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([request.Id], cancellationToken);
        if (parameterGroup is not null)
        {
            parameterGroup.Description = request.Description;
            parameterGroup.ApprovalRequired = request.ApprovalRequired;
            parameterGroup.Code = request.Code;

            var updateResponse = _parameterGroupRepository.Update(parameterGroup);
            await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
            return new ResponseBase<ParameterGroupResult>(updateResponse.ToParameterGroupResult());
        }

        throw new ParameterException("PR001", $"Not found parameter group (Id: {request?.Id})."); ;
    }
}
