using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

public interface IParameterManager
{
    Task<ParameterGroupWithParamsResult> AddParameterGroupWithParamsAsync(AddParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult> AddParameterGroupAsync(AddParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult?> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult?> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParamsAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParamsAsync(string parameterGroupCode, CancellationToken cancellationToken = default);
    Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult> DeleteParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterResult> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default);
    Task<ParameterResult?> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default);
    Task<List<ParameterListItemResult>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default);
    Task<List<ParameterListItemResult>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default);
    Task<ParameterResult> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default);
    Task<ParameterResult> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default);
}