using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

public interface IParameterManager
{
    ParameterGroupResult AddParameterGroup(AddParameterGroupModel model);
    ParameterGroupResult? GetParameterGroup(int parameterGroupId);
    ParameterGroupResult? GetParameterGroup(string parameterCode);
    List<ParameterGroupResult> GetParameterGroupList(int page, int count);
    ParameterGroupResult? UpdateParameterGroup(UpdateParameterGroupModel model);
    bool DeleteParameterGroup(int parameterGroupId);
    Task<ParameterGroupResult> AddParameterGroupAsync(AddParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult?> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult?> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default);
    Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult?> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    ParameterResult AddParameter(AddParameterModel model);
    ParameterResult? GetParameter(int parameterId);
    List<ParameterListItemResult> GetParameters(int groupId);
    List<ParameterListItemResult> GetParameters(string groupCode);
    ParameterResult? UpdateParameter(UpdateParameterModel model);
    bool DeleteParameter(int parameterId);
    Task<ParameterResult> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default);
    Task<ParameterResult?> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default);
    Task<List<ParameterListItemResult>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default);
    Task<List<ParameterListItemResult>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default);
    Task<ParameterResult?> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default);
}