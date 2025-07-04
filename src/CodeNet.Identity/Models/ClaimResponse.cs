namespace CodeNet.Identity.Models;

public class ClaimResponse
{
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required string ValueType { get; set; }
}
