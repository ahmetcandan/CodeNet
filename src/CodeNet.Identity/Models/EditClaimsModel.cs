using System.Security.Claims;

namespace CodeNet.Identity.Models;

public class EditClaimsModel
{
    public required string Type { get; set; }
    public required string Value { get; set; }

    public Claim GetClaim()
    {
        return new(Type, Value);
    }
}
