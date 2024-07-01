using CodeNet.Core.Models;
using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

public interface IParameterManager
{
    Task<ResponseBase<ParameterGroupWithParamsResult>> AddParameterGroupWithParamsAsync(AddParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupResult>> AddParameterGroupAsync(AddParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupResult>> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupResult>> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupWithParamsResult>> GetParameterGroupWithParamsAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupWithParamsResult>> GetParameterGroupWithParamsAsync(string parameterGroupCode, CancellationToken cancellationToken = default);
    Task<ResponseBase<List<ParameterGroupResult>>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupResult>> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterGroupResult>> DeleteParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterResult>> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterResult>> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default);
    Task<ResponseBase<List<ParameterListItemResult>>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default);
    Task<ResponseBase<List<ParameterListItemResult>>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterResult>> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default);
    Task<ResponseBase<ParameterResult>> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default);
}