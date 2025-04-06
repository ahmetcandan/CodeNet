using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

public interface IParameterManager
{
    Task<ParameterGroupWithParamsResult> AddParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> UpdateParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default);
    Task DeleteParameterAsync(string groupCode, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithParamsResult?> GetParameterAsync(string parameterGroupCode, CancellationToken cancellationToken = default);
    Task<ParameterGroupWithDefaultParamResult?> GetParameterDefaultAsync(string parameterGroupCode, CancellationToken cancellationToken = default);
    Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default);
}