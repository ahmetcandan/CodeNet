namespace CodeNet.Parameters.Models;

public class ParameterGroupWithParamsModel : ParameterGroupModel
{
    public required IList<ParameterModel> Parameters { get; set; }
}
