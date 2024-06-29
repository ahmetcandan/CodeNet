namespace CodeNet.Parameters.Models;

public class ParameterListItemResult : UpdateParameterModel
{
    public required string GroupCode { get; set; }
    public bool ApprovalRequired { get; set; }
}
