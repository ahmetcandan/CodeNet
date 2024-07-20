namespace CodeNet.Parameters.Models;

public class ParameterGroupWithParamsResult : ParameterGroupResult
{
    public required IEnumerable<ParameterResult> Parameters { get; set; }
}
