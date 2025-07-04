namespace CodeNet.Identity.Models;

public class ClaimModel(string type, string value)
{
    public string Type { get; set; } = type;
    public string Value { get; set; } = value;
}
