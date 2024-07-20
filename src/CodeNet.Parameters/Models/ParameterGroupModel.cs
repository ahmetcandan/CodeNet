namespace CodeNet.Parameters.Models;

public class ParameterGroupModel
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
    public bool ApprovalRequired { get; set; }
}
