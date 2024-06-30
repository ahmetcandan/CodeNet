using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class UpdateParameterHandler(ParametersDbContext dbContext, IIdentityContext identityContext) : IRequestHandler<UpdateParameterRequest, ResponseBase<ParameterResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);
    private readonly ParameterRepositoryResolver _parameterRepositoryResolver = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterResult>> Handle(UpdateParameterRequest request, CancellationToken cancellationToken)
    {
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(request.GroupId, cancellationToken);
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(approvalRequired);
        var parameter = await parameterRepository.GetAsync([request.Id], cancellationToken);
        if (parameter is not null)
        {
            parameter.Value = request.Value;
            parameter.Code = request.Code;
            var updateResponse = parameterRepository.Update(parameter);
            await parameterRepository.SaveChangesAsync(cancellationToken);
            return new ResponseBase<ParameterResult>(updateResponse.ToParameterResult());
        }

        throw new ParameterException("PR001", $"Not found parameter (Id: {request?.Id})."); ;
    }
}
