using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class DeleteParameterHandler(ParametersDbContext dbContext, IIdentityContext identityContext) : IRequestHandler<DeleteParameterRequest, ResponseBase<ParameterResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);
    private readonly ParameterRepositoryResolver _parameterRepositoryResolver = new(dbContext, identityContext);
    private readonly ParameterTracingRepository _parameterTracingRepository = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterResult>> Handle(DeleteParameterRequest request, CancellationToken cancellationToken)
    {
        var parameter = await _parameterTracingRepository.GetAsync([request.ParameterId], cancellationToken) ?? throw new ParameterException("PR002", $"Not found parameter (Id: {request.ParameterId}).");
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(parameter.GroupId, cancellationToken);
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(approvalRequired);
        parameterRepository.Remove(parameter);
        await parameterRepository.SaveChangesAsync(cancellationToken);
        return new ResponseBase<ParameterResult>(parameter.ToParameterResult());
    }
}
