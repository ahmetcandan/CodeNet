using CodeNet.Parameters.Models;

namespace CodeNet.Parameters.Manager;

internal static class Helper
{
    public static ParameterGroupResult ToParameterGroupResult(this ParameterGroup parameterGroup) => new()
    {
        ApprovalRequired = parameterGroup.ApprovalRequired,
        Code = parameterGroup.Code,
        Description = parameterGroup.Description,
        Id = parameterGroup.Id
    };
    public static ParameterResult ToParameterResult(this Parameter parameter) => new()
    {
        Id = parameter.Id,
        Code = parameter.Code,
        GroupId = parameter.GroupId,
        Value = parameter.Value,
        IsDefault = parameter.IsDefault,
        Order = parameter.Order
    };
}
