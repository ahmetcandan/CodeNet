namespace CodeNet.Parameters.Models;

public class AddParameterModel
{
    public int GroupId { get; set; }
    public required string Code { get; set; }
    public required string Value { get; set; }
}
