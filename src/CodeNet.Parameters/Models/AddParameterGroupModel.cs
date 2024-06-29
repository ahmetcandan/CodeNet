namespace CodeNet.Parameters.Models;

public class AddParameterGroupModel
{
    public required string Code { get; set; }
    public string? Description { get; set; }
    public bool ApprovalRequired { get; set; }
}
