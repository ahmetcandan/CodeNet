using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

public interface IParameterManager
{
    Task<ParameterGroupWithParamsResult> AddParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> UpdateParameterAsync(int parameterGroupId, ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupResult> DeleteParameterAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> GetParameterAsync(int parameterGroupId, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> GetParameterAsync(string parameterGroupCode, CancellationToken cancellationToken = default);
    Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default);
}