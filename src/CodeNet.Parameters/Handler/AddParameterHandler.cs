using Autofac;
using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CodeNet.Parameters.Handler;

internal class AddParameterHandler(ParametersDbContext dbContext, IIdentityContext identityContext, ILifetimeScope lifetimeScope, IDistributedCache distributedCache) : IRequestHandler<AddParameterRequest, ResponseBase<ParameterResult>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);
    private readonly ParameterRepositoryResolver _parameterRepositoryResolver = new(dbContext, identityContext);

    public async Task<ResponseBase<ParameterResult>> Handle(AddParameterRequest request, CancellationToken cancellationToken)
    {
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(request.GroupId, cancellationToken);
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(approvalRequired);
        var parameter = new Parameter
        {
            Code = request.Code,
            GroupId = request.GroupId,
            Value = request.Value,
        };

        var addResponse = await parameterRepository.AddAsync(parameter, cancellationToken);
        await parameterRepository.SaveChangesAsync(cancellationToken);
        await ClearCache(addResponse);
        return new ResponseBase<ParameterResult>(addResponse.ToParameterResult());
    }

    private async Task ClearCache(Parameter parameter)
    {
        try
        {
            #region GetParameter
            var methodBase = ParameterDecorator<GetParameterRequest, ResponseBase<ParameterResult>>.GetHandlerMethodInfo(lifetimeScope);
            if (methodBase is not null)
            {
                string key = ParameterDecorator<GetParameterRequest, ResponseBase<ParameterResult>>.GetKey(methodBase, new GetParameterRequest { ParameterId = parameter.Id });
                await distributedCache.RemoveAsync(key);
            }
            #endregion

            #region GetParameters
            methodBase = ParameterDecorator<GetParametersRequest, ResponseBase<List<ParameterListItemResult>>>.GetHandlerMethodInfo(lifetimeScope);
            if (methodBase is not null)
            {
                string key = ParameterDecorator<GetParametersRequest, ResponseBase<List<ParameterListItemResult>>>.GetKey(methodBase, new GetParametersRequest { ParameterGroupId = parameter.GroupId });
                await distributedCache.RemoveAsync(key);
            }
            #endregion
        }
        catch { }
    }
}
