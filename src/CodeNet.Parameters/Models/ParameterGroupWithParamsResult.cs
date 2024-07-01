namespace CodeNet.Parameters.Models;

public class ParameterGroupWithParamsResult : ParameterGroupResult
{
    public required List<ParameterResult> Parameters { get; set; }
}
