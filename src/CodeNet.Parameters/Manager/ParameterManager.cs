using CodeNet.Core.Models;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Manager;

public class ParameterManager(IMediator mediator) : IParameterManager
{
    #region Parameter Group CRUD
    public Task<ResponseBase<ParameterGroupResult>> AddParameterGroupAsync(AddParameterGroupModel model, CancellationToken cancellationToken = default)
    {
        return mediator.Send((AddParameterGroupRequest)model, cancellationToken);
    }

    public Task<ResponseBase<ParameterGroupResult>> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParameterGroupRequest { ParameterGroupId = parameterGroupId }, cancellationToken);
    }

    public Task<ResponseBase<ParameterGroupResult>> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParameterGroupRequest { ParameterGroupCode = parameterCode }, cancellationToken);
    }

    public Task<ResponseBase<List<ParameterGroupResult>>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParameterGroupListRequest { Page = page, Count = count }, cancellationToken);
    }

    public Task<ResponseBase<ParameterGroupResult>> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default)
    {
        return mediator.Send((UpdateParameterGroupRequest)model, cancellationToken);
    }

    public Task<ResponseBase<ParameterGroupResult>> DeleteParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new DeleteParameterGroupRequest { ParameterGroupId = parameterGroupId }, cancellationToken);
    }
    #endregion

    #region Parameter CRUD
    public Task<ResponseBase<ParameterResult>> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default)
    {
        return mediator.Send((AddParameterRequest)model, cancellationToken);
    }

    public Task<ResponseBase<ParameterResult>> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParameterRequest { ParameterId = parameterId }, cancellationToken);
    }

    public Task<ResponseBase<List<ParameterListItemResult>>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParametersRequest { ParameterGroupId = groupId }, cancellationToken);
    }

    public Task<ResponseBase<List<ParameterListItemResult>>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new GetParametersRequest { ParameterGroupCode = groupCode }, cancellationToken);
    }

    public Task<ResponseBase<ParameterResult>> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default)
    {
        return mediator.Send((UpdateParameterRequest)model, cancellationToken);
    }

    public Task<ResponseBase<ParameterResult>> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        return mediator.Send(new DeleteParameterRequest { ParameterId = parameterId }, cancellationToken);
    }
    #endregion
}
