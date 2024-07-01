namespace CodeNet.Parameters.Models;

public class AddParameterGroupWithParamsModel : AddParameterGroupModel
{
    public required List<AddParameterModel> AddParameters { get; set; }
}
