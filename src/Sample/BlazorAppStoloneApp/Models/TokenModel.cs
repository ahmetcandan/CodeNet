namespace CodeNetUI_Example.Models;

public class TokenModel
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime CreatedDate { get; set; }
    public IEnumerable<ClaimResponse> Claims { get; set; }
}

public class ClaimResponse
{
    public string Type { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
}
