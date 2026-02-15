namespace CodeNet.Parameters.Models;

public class ParameterModel
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Value { get; set; }
    public bool IsDefault { get; set; }
    public byte Order { get; set; }
}
